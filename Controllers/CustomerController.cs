using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ILogger<CustomerController> _logger)
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
                logger.LogError(error, "Customer Controller - Index");
                throw error;
            }
        }
    }
}
