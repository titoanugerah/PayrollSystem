using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class PayrollHistoryController : Controller
    {
        private readonly ILogger<PayrollHistoryController> logger;
        
        public PayrollHistoryController(ILogger<PayrollHistoryController> _logger)
        {
            logger = _logger;
        }
        public async Task<IActionResult> Index()
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

        [Route("PayrollHistory/Download/Report/{id}")]
        public async Task<IActionResult> DownloadReport(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Download Report");
                throw error;
            }
        }
    }
}
