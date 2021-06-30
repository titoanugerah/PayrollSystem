using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Payroll.Controllers
{
    public class PayrollHistoryController : Controller
    {
        private readonly ILogger<PayrollHistoryController> logger;
        
        public PayrollHistoryController(ILogger<PayrollHistoryController> _logger)
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
