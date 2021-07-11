using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Payroll.Models;
using Payroll.ViewModels;
using Payroll.WebSockets;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IHubContext<NotificationHub> notificationHub;
        public HomeController(ILogger<HomeController> _logger, IHubContext<NotificationHub> _notificationHub)
        {
            logger = _logger;
            notificationHub = _notificationHub;
        }

        public IActionResult Index()
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
