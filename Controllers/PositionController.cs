using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class PositionController : Controller
    {
        private readonly ILogger<PositionController> logger;

        public PositionController(ILogger<PositionController> _logger)
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
                logger.LogError(error, $"Position Controller - Index");
                throw error;
            }
        }
    }
}
