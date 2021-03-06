using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> logger;
        public DashboardController(ILogger<DashboardController> _logger)
        {
            logger = _logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Dashboard Controller - Index");
                throw error;
            }
        }
    }
}
