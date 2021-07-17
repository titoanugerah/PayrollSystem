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
        private readonly PayrollDB payrollDB;

        public AuthController(ILogger<AuthController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        [Route("Auth/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth Controller - Login");
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
