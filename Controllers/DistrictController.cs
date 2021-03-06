using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class DistrictController : Controller
    {
        private readonly ILogger<DistrictController> logger;
        public DistrictController(ILogger<DistrictController> _logger)
        {
            logger = _logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District Controller - Index");
                throw error;
            }
        }
    }
}
