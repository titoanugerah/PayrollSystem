using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class PayrollDetailController : Controller
    {
        private readonly ILogger<PayrollDetailController> logger;
        private readonly PayrollDB payrollDB;
        public PayrollDetailController(ILogger<PayrollDetailController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Route("PayrollDetail")]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                PayrollHistory payrollHistory = await payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                ViewBag.Period = $"{UcFirst(payrollHistory.Month)}, {payrollHistory.Year}";
                ViewBag.Id = id;
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"PayrollDetail Controller - Index");
                throw error;
            }
        }

        private string UcFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
