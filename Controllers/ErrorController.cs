using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.ViewModels;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Payroll.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> _logger)
        {
            logger = _logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error/{code:int}")]
        public IActionResult Handler(int code)
        {
            switch (code)
            {
                case 400:
                    ViewBag.errorMessage = "Alamat yang anda ketik salah, silakan periksa kembali";
                    break;
                case 401:
                    ViewBag.errorMessage = "Anda tidak memiliki hak untuk mengakses halaman ini";
                    break;
                case 403:
                    ViewBag.errorMessage = "Anda tidak dapat mengakses halaman ini";
                    break;
                case 404:
                    ViewBag.errorMessage = "Halaman yang anda cari tidak tersedia";
                    break;
                default:
                    ViewBag.errorMessage = $"Terdapat kesalahan dari aplikasi kode {code}";
                    break;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            var exceptionHandlerFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (exceptionHandlerFeature != null)
            {
                logger.LogError(exceptionHandlerFeature.Error, "Error", errorViewModel);
            }
            return View(errorViewModel);
        }
    }
}
