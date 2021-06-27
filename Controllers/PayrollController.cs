using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Payroll.Controllers
{
    public class PayrollController : Controller
    {
        private readonly ILogger<PayrollController> logger;
        
        public PayrollController(ILogger<PayrollController> _logger)
        {
            logger = _logger;
        }
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Controller - Index");
                throw error;
            }
        }
    }
}
