using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Payroll.Controllers
{
    public class MainCustomerController : Controller
    {
        private readonly ILogger<MainCustomerController> logger;
        public MainCustomerController(ILogger<MainCustomerController> _logger)
        {
            logger = _logger;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Main Customer Controller - Index");
                throw error;
            }
        }
    }
}
