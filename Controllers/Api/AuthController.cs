using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using Payroll.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PayrollDB payrollDB;

        public AuthController(ILogger<AuthController> _logger, IHttpContextAccessor _httpContextAccessor, PayrollDB _payrollDB)
        {
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
            payrollDB = _payrollDB;
        }

        [Authorize]
        [HttpPost]
        [Route("api/auth/updatepassword")]
        public async Task<IActionResult> UpdatePassword([FromForm]UpdatePasswordInput updatePasswordInput)
        {
            try
            {
                Employee employee = payrollDB.Employee
                    .Where(column => column.Id == httpContextAccessor.HttpContext.User.GetEmployeeId())
                    .FirstOrDefault();
                employee.KTP = updatePasswordInput.KTP;
                employee.Password = CreateMd5Hash(updatePasswordInput.Password);
                payrollDB.Entry(employee).State = EntityState.Modified;
                payrollDB.Employee.Update(employee);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth API Controller - Update Password");
                return BadRequest(error);
            }
        }

        private static string CreateMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        [Authorize]
        [HttpPost]
        [Route("api/auth/getKTP")]
        public async Task<IActionResult> GetKTP()
        {
            try
            {
                string ktp = payrollDB.Employee
                    .Where(column => column.Id == httpContextAccessor.HttpContext.User.GetEmployeeId())
                    .Select(column => column.KTP)
                    .FirstOrDefault();
                return new JsonResult(ktp);                
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth API Controller - Get KTP ");
                throw;
            }
        }
    }
}
