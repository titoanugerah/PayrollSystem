using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> logger;
        public EmployeeController(ILogger<EmployeeController> _logger)
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
                logger.LogError(error, "Customer Controller - Index");
                throw error;
            }

}
    }
}
