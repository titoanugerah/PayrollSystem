using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Payroll.DataAccess;
using Payroll.Models;
using Payroll.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class PayrollDetailController : Controller
    {
        private readonly ILogger<PayrollDetailController> logger;
        private readonly PayrollDB payrollDB;
        private readonly IHttpContextAccessor httpContextAccessor;
        private ExcelWorksheet worksheet;
        public PayrollDetailController(ILogger<PayrollDetailController> _logger, PayrollDB _payrollDB, IHttpContextAccessor _httpContextAccessor)
        {
            logger = _logger;
            payrollDB = _payrollDB;
            httpContextAccessor = _httpContextAccessor;
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index(int id)
        {
            try
            {
                PayrollHistory payrollHistory = await payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                ViewBag.Period = $"{UcFirst(payrollHistory.Month)}, {payrollHistory.Year}";
                ViewBag.Id = id;
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"PayrollDetail Controller - Index");
                throw error;
            }
        }

        [Route("PayrollDetail/Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                FileInfo excelTemplate = new FileInfo("wwwroot/file/TemplateSlip.xlsx");
                PayrollDetail payrollDetail = await payrollDB.PayrollDetail
                    .Include(table => table.Employee.Customer)
                    .Include(table => table.Employee.Location)
                    .Include(table => table.Employee.Position)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelTemplate.FullName)))
                {
                    ExcelWorkbook workbook = excelPackage.Workbook;
                    worksheet = workbook.Worksheets.FirstOrDefault();

                    await SetValue($"D1", $"{payrollDetail.PayrollHistory.Month}, {payrollDetail.PayrollHistory.Year}");
                    await SetValue($"H1", $"{payrollDetail.Employee.Name}");
                    //TODO
                    //await SetValue($"H2", $"{payrollDetail.Employee.NIK}");
                    await SetValue($"H3", $"{payrollDetail.Employee.Location.Name}");                    

                    await SetValue($"D7", $"{payrollDetail.MainSalaryBilling}");
                    await SetValue($"D8", $"{payrollDetail.OvertimeBilling}");
                    await SetValue($"D9", $"{payrollDetail.AttendanceBilling}");
                    await SetValue($"D10", $"{payrollDetail.InsentiveBilling}");
                    await SetValue($"D11", $"{payrollDetail.AppreciationBilling}");

                    await SetValue($"D14", $"{payrollDetail.MainSalaryBilling + payrollDetail.OvertimeBilling + payrollDetail.AttendanceBilling + payrollDetail.InsentiveBilling + payrollDetail.AppreciationBilling}");

                    await SetValue($"H7", $"{payrollDetail.BpjsTkDeduction}");
                    await SetValue($"H8", $"{payrollDetail.BpjsKesehatanDeduction}");
                    await SetValue($"H9", $"{payrollDetail.PensionDeduction}");
                    await SetValue($"H10", $"{payrollDetail.PPH21}");
                    await SetValue($"H11", $"{payrollDetail.AnotherDeduction}");
                    await SetValue($"H12", $"{payrollDetail.TransferFee}");
                    
                    await SetValue($"H14", $"{payrollDetail.BpjsKesehatanDeduction + payrollDetail.BpjsTkDeduction + payrollDetail.PensionDeduction + payrollDetail.PPH21 + payrollDetail.AnotherDeduction +  payrollDetail.TransferFee}");
                    await SetValue($"H12", $"{payrollDetail.TakeHomePay}");

                    MemoryStream stream = new MemoryStream();
                    excelPackage.SaveAs(stream);

                    //Workbook workbooks = new Workbook();
                    //workbooks.LoadFromFile("sample.xlsx", ExcelVersion.Version2007);
                    //PdfConverter pdfConverter = new PdfConverter(workbook);

                    byte[] content = stream.ToArray();
                    return File(content, "application/xls", $"Transferan.pdf");

                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payrolll History Controller - Download Report Bank {id}");
                throw error;
            }
        }

        [Route("PayrollDetail/Download/Slip/{id}")]
        public async Task<IActionResult> DownloadSlip(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new JsonResult("");
                }
                UserIdentity userIdentity = httpContextAccessor.HttpContext.User.GetUserIdentity();
                PayrollDetail payrollDetail = payrollDB.PayrollDetail
                    .Include(table => table.Employee.Location)
                    .Include(table => table.Employee.Customer)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                if (payrollDetail.EmployeeId == userIdentity.NIK || userIdentity.Role.ToLower() == "admin")
                {
                    ViewBag.payrollDetail = payrollDetail;
                    return View();
                }
                else
                {
                    return RedirectToAction("error/401", "error");
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail Controller - Download Slip {id}");
                throw error;
            }
        }

        private async Task<IActionResult> SetValue(string cell, object value)
        {
            try
            {
                worksheet.Cells[$"{cell}"].Value = value;
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Set Value");
                throw error;
            }
        }


        private string UcFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
