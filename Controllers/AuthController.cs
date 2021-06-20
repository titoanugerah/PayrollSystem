using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> logger;
        private readonly DatabaseContext dbContext;

        public AuthController(ILogger<AuthController> _logger, DatabaseContext _dbContext)
        {
            logger = _logger;
            dbContext = _dbContext;
        }

        [Route("Auth/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                var properties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    RedirectUri = Url.Action("Validate")
                };
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth Controller - Login");
                throw;
            }
        }

        [AllowAnonymous]
        [Route("Auth/Validate")]
        public async Task<IActionResult> Validate()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                List<ViewModels.UserClaim> userClaim = result.Principal
                    .Identities.FirstOrDefault().Claims
                    .Where(claim => claim.Type == "email" || claim.Type == "name" || claim.Type == "picture")
                    .Select(claim => new ViewModels.UserClaim
                    {
                        Type = claim.Type,
                        Value = claim.Value
                    })
                    .ToList();
                ViewModels.UserIdentity userIdentity = new ViewModels.UserIdentity();
                userIdentity.Email = userClaim.Where(claim => claim.Type == "email").Select(claim => claim.Value).FirstOrDefault();
                userIdentity.Picture = userClaim.Where(claim => claim.Type == "picture").Select(claim => claim.Value).FirstOrDefault();
                userIdentity.Name = userClaim.Where(claim => claim.Type == "name").Select(claim => claim.Value).FirstOrDefault();
                var checkUser = dbContext.Employee
                    .Any(user => user.Email == userIdentity.Email);
                if (checkUser)
                {
                    var user = dbContext.Employee
                        .Include(table => table.Role)
                        .Where(user => user.Email == userIdentity.Email)
                        .FirstOrDefault();
                    user.Image = userIdentity.Picture;
                    user.Name = userIdentity.Name;
                    await dbContext.SaveChangesAsync();
                    var claims = new List<Claim>
                {
                    new Claim("Id", user.NIK.ToString()),
                    new Claim("Name", user.Name),
                    new Claim("Email", user.Email),
                    new Claim("Role", user.Role.Name),
                    new Claim("Image", user.Image),
                    new Claim("RoleId", user.RoleId.ToString())
                };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    return RedirectToAction("Index", "Home");
                }
                return Json("User Not Registered");
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth Controller - Validate ");
                throw;
            }

        }

        [Authorize]
        [Route("Auth/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception error)
            {
                logger.LogError(error, "Account Controller - Logout");
                throw;
            }
        }

    }
}
