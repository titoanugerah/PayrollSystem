using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly DatabaseContext dbContext;

        public HomeController(ILogger<HomeController> _logger, DatabaseContext _dbContext)
        {
            logger = _logger;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
