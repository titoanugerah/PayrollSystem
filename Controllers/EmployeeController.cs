using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
    }
}
