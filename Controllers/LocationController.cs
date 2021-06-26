using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILogger<LocationController> logger;
        public LocationController(ILogger<LocationController> _logger)
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
                logger.LogError(error, "Location Controller - Index");
                throw error;
            }
        }
    }
}
