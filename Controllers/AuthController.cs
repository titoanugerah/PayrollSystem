using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using Payroll.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
                throw error;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Auth/Validate")]
        public async Task<IActionResult> Validate(LoginInput loginInput)
        {
            try
            {
                bool isAnyEmployee = false;
                Employee employee = new Employee();
                if (loginInput.Username == null || loginInput.Password == null)
                {
                    ViewBag.Message = $"Kolom NIK /Pasword kosong, silahkan dilengkapi";
                    return View("Login");
                }

                if (isAnyEmployee = await payrollDB.Employee.Where(column => column.PrimaryNIK.ToString() == loginInput.Username).AnyAsync())
                {
                    employee = await payrollDB.Employee
                        .Where(column => column.PrimaryNIK.ToString() == loginInput.Username)
                        .FirstOrDefaultAsync();                    
                } 
                else if (isAnyEmployee = await payrollDB.Employee.Where(column => column.SecondaryNIK.ToLower() == loginInput.Username.ToLower()).AnyAsync())
                {
                    employee = await payrollDB.Employee
                        .Include(table => table.Role)
                        .Where(column => column.SecondaryNIK.ToLower() == loginInput.Username.ToLower())
                        .FirstOrDefaultAsync();
                }
                else
                {
                    ViewBag.Message = $"Username tidak ditemukan";
                    return View("Login");
                }

                string encryptedPassword = null;
                using (MD5 md5Hash = MD5.Create())
                {
                    encryptedPassword = GetMd5Hash(md5Hash, loginInput.Password);
                }

                if (employee.Password == encryptedPassword)
                {
                    List<Claim> userClaims = new List<Claim>()
                    {
                        
                        new Claim("Id", employee.Id.ToString()),
                        new Claim(ClaimTypes.Name, employee.Name),
                        new Claim(ClaimTypes.Role, employee.Role.Name),
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                    return RedirectToAction("Index", "Dashboard" );
                }
                else
                {
                    ViewBag.Message = $"Kombinasi nik dan password anda keliru, silahkan cek kembali";
                    return View("Login");
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth Controller - Validation");
                ViewBag.Message = $"Error : {error.Message}";
                return View("Login");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Reset")]
        public async Task<IActionResult> Reset(ResetInput resetInput)
        {
            try
            {
                Employee employee = await payrollDB.Employee
                    .Where(column => column.Name == resetInput.Name)
                    .FirstOrDefaultAsync();
                if (employee != null)
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        employee.Password = GetMd5Hash(md5Hash, resetInput.Password);
                        payrollDB.Entry(employee).State = EntityState.Modified;
                        payrollDB.Employee.Update(employee);
                        await payrollDB.SaveChangesAsync();
                        ViewBag.Message = $"Password berhasil direset sesuai dengan NIK";
                    }
                }
                else
                {
                    ViewBag.Message = $"Kombinasi NIK dan nomor Nama anda salah, silahkan ulangi kembali";
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Auth Controller - Reset");
                ViewBag.Message = error.Message;
            }
            return View("Login");
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        [Authorize]
        [Route("Auth/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception error)
            {
                logger.LogError(error, "Account Controller - Logout");
                throw error;
            }
        }

    }
}
