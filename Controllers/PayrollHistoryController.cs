using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Payroll.DataAccess;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers
{
    public class PayrollHistoryController : Controller
    {
        private readonly ILogger<PayrollHistoryController> logger;
        private readonly PayrollDB payrollDB;
        private ExcelWorksheet worksheet;

        public PayrollHistoryController(ILogger<PayrollHistoryController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
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
                logger.LogError(error, "Payroll Controller - Index");
                throw error;
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assa()
        {
            try
            {
                //Default Id Assa
                ViewBag.MainCustomerId = 1;
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Controller - Assa");
                throw error;
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Syncrum()
        {
            try
            {
                //Default Id Syncrum
                ViewBag.MainCustomerId = 3;
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Controller - Syncrum");
                throw error;
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TTNT()
        {
            try
            {
                //Default Id TTNT
                ViewBag.MainCustomerId = 2;
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Controller - TTNT");
                throw error;
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dharma()
        {
            try
            {
                //Default Id Dharma
                ViewBag.MainCustomerId = 4;
                return View();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Controller - Dharma");
                throw error;
            }
        }

        [Route("PayrollHistory/Download/ReportBank/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReportBank(int id)
        {
            try
            {
                FileInfo excelTemplate = new FileInfo("wwwroot/file/templateBank.xlsx");
                List<PayrollDetail> allPayrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.Employee.Customer)
                    .Include(table => table.Employee.Location)
                    .Include(table => table.Employee.Position)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.PayrollHistoryId == id)
                    .Where(column => column.Employee.IsExist == true)
                    .Where(column => column.IsExist == true)
                    .OrderBy(column => column.Employee.CustomerId)
                    .ToListAsync();
                List<Bank> banks = await payrollDB.Bank
                    .ToListAsync();
                List<PayrollDetail> payrollBCA = allPayrollDetails
                    .Where(column => column.Employee.BankCode == "BCA")
                    .ToList();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelTemplate.FullName)))
                {
                    ExcelWorkbook workbook = excelPackage.Workbook;
                    ExcelWorksheet template = workbook.Worksheets.FirstOrDefault();

                    int currentRow = 6;
                    int no = 1;
                    string worksheetName = $"BANK DRIVER";
                    worksheet = workbook.Worksheets.Copy(template.ToString(), worksheetName);
                    foreach (PayrollDetail payrollDetail in payrollBCA)
                    {

                        await SetValue($"A{currentRow}", no);
                        //TODO
                        //await SetValue($"B{currentRow}", payrollDetail.Employee.NIK);
                        await SetValue($"C{currentRow}", payrollDetail.Employee.Name);
                        await SetValue($"D{currentRow}", payrollDetail.Employee.Position.Name);
                        await SetValue($"E{currentRow}", payrollDetail.Employee.Location.Name);
                        await SetValue($"F{currentRow}", payrollDetail.Employee.Customer.Name);
                        await SetValue($"G{currentRow}", payrollDetail.TakeHomePay);
                        await SetValue($"H{currentRow}", 0);
                        await SetValue($"I{currentRow}", payrollDetail.TakeHomePay);
                        await SetValue($"J{currentRow}", payrollDetail.Employee.AccountNumber);
                        await SetValue($"K{currentRow}", payrollDetail.Employee.BankCode);
                        no++;
                        currentRow++;
                    }
                    await HideColumn(2);
                    await Borderize($"A6", $"K{currentRow}");


                    currentRow = 6;
                    no = 1;
                    worksheetName = $"BANK LAIN";
                    worksheet = workbook.Worksheets.Copy(template.ToString(), worksheetName);
                    foreach (PayrollDetail payrollDetail in allPayrollDetails.Where(col => col.Employee.BankCode != "BCA"))
                    {

                        await SetValue($"A{currentRow}", no);
                        //TODO
                        //await SetValue($"B{currentRow}", payrollDetail.Employee.NIK);
                        await SetValue($"C{currentRow}", payrollDetail.Employee.Name);
                        await SetValue($"D{currentRow}", payrollDetail.Employee.Position.Name);
                        await SetValue($"E{currentRow}", payrollDetail.Employee.Location.Name);
                        await SetValue($"F{currentRow}", payrollDetail.Employee.Customer.Name);
                        await SetValue($"G{currentRow}", payrollDetail.TakeHomePay - 6500);
                        await SetValue($"H{currentRow}", 6500);
                        await SetValue($"I{currentRow}", payrollDetail.TakeHomePay);
                        await SetValue($"J{currentRow}", payrollDetail.Employee.AccountNumber);
                        await SetValue($"K{currentRow}", payrollDetail.Employee.BankCode);
                        no++;
                        currentRow++;
                    }
                    await HideColumn(2);
                    await Borderize($"A6", $"K{currentRow}");
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    excelPackage.Workbook.Worksheets.Delete(template);
                    MemoryStream stream = new MemoryStream();
                    excelPackage.SaveAs(stream);
                    byte[] content = stream.ToArray();
                    return File(content, "application/excel", $"Transferan .xlsx");

                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payrolll History Controller - Download Report Bank {id}");
                throw error;
            }
        }

        [Route("PayrollHistory/Download/Report/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReportAll(int id)
        {
            try
            {
                FileInfo excelTemplate = new FileInfo("wwwroot/file/templateReport.xlsx");
                List<PayrollDetail> allPayrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.Employee.Customer)
                    .Include(table => table.Employee.Location)
                    .Include(table => table.Employee.Position)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.PayrollHistoryId == id)
                    .OrderBy(column => column.Employee.CustomerId)
                    .ToListAsync();
                if (allPayrollDetails.Count() == 0)
                {
                    return new JsonResult("No Data Received");
                }
                List<Customer> customers = await payrollDB.Customer
                    .Where(column => allPayrollDetails.Select(col => col.Employee.CustomerId).Contains(column.Id))
                    .ToListAsync();
                List<Location> allLocations = await payrollDB.Location
                    .ToListAsync();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelTemplate.FullName)))
                {
                    ExcelWorkbook workbook = excelPackage.Workbook;
                    ExcelWorksheet template = workbook.Worksheets.FirstOrDefault();

                    foreach (Customer customer in customers)
                    {

                        List<Location> locations = allLocations
                          .Where(col => allPayrollDetails.Select(column => column.Employee.LocationId).Contains(col.Id))
                          .Where(col => allPayrollDetails.Select(column => column.Employee.CustomerId).Contains(customer.Id))
                          .ToList();
                        foreach (Location location in locations)
                        {

                            string worksheetName = $"{customer.Name} {location.Name}";
                            worksheet = workbook.Worksheets.Copy(template.ToString(), worksheetName);
                            await Merge($"A1", $"C1");
                            await SetValue($"A2", $"Tagihan Driver {location.Name} {customer.Name}");
                            await Merge($"A2", $"C2");
                            await SetValue($"A3", $"Periode {allPayrollDetails.First().PayrollHistory.Month} {allPayrollDetails.First().PayrollHistory.Year} ");
                            await Merge($"A3", $"C3");
                            List<PayrollDetail> payrollDetails = allPayrollDetails
                              .Where(col => col.Employee.CustomerId == customer.Id)
                              .Where(col => col.Employee.LocationId == location.Id)
                              .ToList();
                            int currentRow = 7;
                            int no = 1;
                            foreach (PayrollDetail payrollDetail in payrollDetails)
                            {
                                await SetValue($"A{currentRow}", no);
                                //await SetValue($"B{currentRow}", payrollDetail.Employee.NIK);
                                await SetValue($"C{currentRow}", payrollDetail.Employee.Name);
                                await SetValue($"D{currentRow}", payrollDetail.Employee.Position.Name);
                                await SetValue($"E{currentRow}", payrollDetail.Employee.Location.Name);
                                await SetValue($"F{currentRow}", payrollDetail.Employee.Customer.Name);
                                await SetValue($"G{currentRow}", payrollDetail.MainSalaryBilling);
                                await SetValue($"H{currentRow}", payrollDetail.JamsostekBilling);
                                await SetValue($"I{currentRow}", payrollDetail.BpjsBilling);
                                await SetValue($"J{currentRow}", payrollDetail.PensionBilling);
                                await SetValue($"K{currentRow}", payrollDetail.AtributeBilling);
                                await SetValue($"L{currentRow}", payrollDetail.MainPrice);
                                await SetValue($"M{currentRow}", payrollDetail.ManagementFeeBilling);
                                await SetValue($"N{currentRow}", payrollDetail.InsentiveBilling);
                                await SetValue($"O{currentRow}", payrollDetail.AttendanceBilling);
                                await SetValue($"P{currentRow}", payrollDetail.AppreciationBilling);
                                await SetValue($"Q{currentRow}", payrollDetail.OvertimeBilling);
                                await SetValue($"R{currentRow}", payrollDetail.SubtotalBilling);
                                await SetValue($"S{currentRow}", payrollDetail.TaxBilling);
                                await SetValue($"T{currentRow}", payrollDetail.GrandTotalBilling);
                                //await SetValue($"U{currentRow}", payrollDetail.Employee.JoinCustomerDate);

                                await SetValue($"X{currentRow}", no);
                                //TODO
                                //await SetValue($"Y{currentRow}", payrollDetail.Employee.NIK);
                                await SetValue($"Z{currentRow}", payrollDetail.Employee.Name);
                                await SetValue($"AA{currentRow}", payrollDetail.Employee.FamilyStatusCode);
                                await SetValue($"AB{currentRow}", payrollDetail.ResultPayroll);
                                await SetValue($"AC{currentRow}", payrollDetail.Rapel);
                                await SetValue($"AD{currentRow}", payrollDetail.BpjsReturn);
                                await SetValue($"AE{currentRow}", payrollDetail.FeePayroll);
                                await SetValue($"AF{currentRow}", payrollDetail.TotalPayroll);
                                await SetValue($"AG{currentRow}", payrollDetail.TaxPayroll);
                                await SetValue($"AH{currentRow}", payrollDetail.GrossPayroll);
                                await SetValue($"AI{currentRow}", payrollDetail.AttributePayroll);
                                await SetValue($"AJ{currentRow}", payrollDetail.BpjsTkDeduction);
                                await SetValue($"AK{currentRow}", payrollDetail.BpjsKesehatanDeduction);
                                await SetValue($"AL{currentRow}", payrollDetail.PensionDeduction);
                                await SetValue($"AM{currentRow}", payrollDetail.PTKP);
                                await SetValue($"AN{currentRow}", payrollDetail.PKP1);
                                await SetValue($"AO{currentRow}", payrollDetail.PKP2);
                                await SetValue($"AP{currentRow}", payrollDetail.PPH21);
                                await SetValue($"AQ{currentRow}", payrollDetail.PPH23);
                                await SetValue($"AR{currentRow}", payrollDetail.Netto);
                                await SetValue($"AS{currentRow}", payrollDetail.AnotherDeduction);
                                await SetValue($"AT{currentRow}", payrollDetail.TakeHomePay);
                                
                                await SetValue($"AV{currentRow}", no);
                                //TODO
                                //await SetValue($"AW{currentRow}", payrollDetail.Employee.NIK);
                                await SetValue($"AX{currentRow}", payrollDetail.Employee.Name);
                                await SetValue($"AY{currentRow}", payrollDetail.TakeHomePay);
                                currentRow++;
                                no++;
                            }

                            await SetValue($"A{currentRow}", $"Total");
                            await Merge($"A{currentRow}", $"F{currentRow}");
                            await SetValue($"G{currentRow}", payrollDetails.Sum(column => column.MainSalaryBilling));
                            await SetValue($"H{currentRow}", payrollDetails.Sum(column => column.JamsostekBilling));
                            await SetValue($"I{currentRow}", payrollDetails.Sum(column => column.BpjsBilling));
                            await SetValue($"J{currentRow}", payrollDetails.Sum(column => column.PensionBilling));
                            await SetValue($"K{currentRow}", payrollDetails.Sum(column => column.AtributeBilling));
                            await SetValue($"L{currentRow}", payrollDetails.Sum(column => column.MainPrice));
                            await SetValue($"M{currentRow}", payrollDetails.Sum(column => column.ManagementFeeBilling));
                            await SetValue($"N{currentRow}", payrollDetails.Sum(column => column.InsentiveBilling));
                            await SetValue($"O{currentRow}", payrollDetails.Sum(column => column.AttendanceBilling));
                            await SetValue($"P{currentRow}", payrollDetails.Sum(column => column.AppreciationBilling));
                            await SetValue($"Q{currentRow}", payrollDetails.Sum(column => column.OvertimeBilling));
                            await SetValue($"R{currentRow}", payrollDetails.Sum(column => column.SubtotalBilling));
                            await SetValue($"S{currentRow}", payrollDetails.Sum(column => column.TaxBilling));
                            await SetValue($"T{currentRow}", payrollDetails.Sum(column => column.GrandTotalBilling));

                            await SetValue($"AB{currentRow}", payrollDetails.Sum(column => column.ResultPayroll));
                            await SetValue($"AC{currentRow}", payrollDetails.Sum(column => column.FeePayroll));
                            await SetValue($"AD{currentRow}", payrollDetails.Sum(column => column.TotalPayroll));
                            await SetValue($"AE{currentRow}", payrollDetails.Sum(column => column.TaxPayroll));
                            await SetValue($"AF{currentRow}", payrollDetails.Sum(column => column.GrossPayroll));
                            await SetValue($"AG{currentRow}", payrollDetails.Sum(column => column.AttributePayroll));
                            await SetValue($"AH{currentRow}", payrollDetails.Sum(column => column.BpjsTkDeduction));
                            await SetValue($"AI{currentRow}", payrollDetails.Sum(column => column.BpjsKesehatanDeduction));
                            await SetValue($"AJ{currentRow}", payrollDetails.Sum(column => column.PensionDeduction));
                            await SetValue($"AK{currentRow}", payrollDetails.Sum(column => column.PTKP));
                            await SetValue($"AL{currentRow}", payrollDetails.Sum(column => column.PKP1));
                            await SetValue($"AM{currentRow}", payrollDetails.Sum(column => column.PKP2));
                            await SetValue($"AN{currentRow}", payrollDetails.Sum(column => column.PPH21));
                            await SetValue($"AO{currentRow}", payrollDetails.Sum(column => column.PPH23));
                            await SetValue($"AP{currentRow}", payrollDetails.Sum(column => column.Netto));
                            await SetValue($"AQ{currentRow}", payrollDetails.Sum(column => column.AnotherDeduction));
                            await SetValue($"AR{currentRow}", payrollDetails.Sum(column => column.TakeHomePay));

                            await SetValue($"AW{currentRow}", payrollDetails.Sum(column => column.TakeHomePay));

                            await Borderize($"A6", $"V{currentRow}");
                            await Borderize($"X6", $"AR{currentRow}");
                            await Borderize($"AT6", $"AW{currentRow}");

                            currentRow = currentRow + 2;
                            await SetValue($"AB{currentRow}", $"BPJS TK");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.JamsostekBilling)) + (payrollDetails.Sum(column => column.PensionBilling)) + (payrollDetails.Sum(column => column.BpjsTkDeduction)) + (payrollDetails.Sum(column => column.PensionDeduction)));
                            
                            await SetValue($"AB{currentRow}", $"BPJS Kes");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.BpjsKesehatanDeduction)) + (payrollDetails.Sum(column => column.BpjsBilling)));

                            await SetValue($"AB{currentRow}", $"Perlengkapan");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.AtributeBilling)));

                            await SetValue($"AB{currentRow}", $"PPH 21");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.PPH21)));

                            await SetValue($"AB{currentRow}", $"Potongan");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.AnotherDeduction)));

                            await SetValue($"AB{currentRow}", $"Fee");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.ManagementFeeBilling)));
                            
                            await SetValue($"AB{currentRow}", $"PPN");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.TaxBilling)));

                            await SetValue($"AB{currentRow}", $"TF");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.TakeHomePay)));

                            await SetValue($"AB{currentRow}", $"Total");
                            await SetValue($"AC{currentRow++}", payrollDetails.Sum(column => column.TakeHomePay) + payrollDetails.Sum(column => column.AtributeBilling) + payrollDetails.Sum(column => column.JamsostekBilling) + (payrollDetails.Sum(column => column.PensionBilling)) + (payrollDetails.Sum(column => column.BpjsTkDeduction)) + payrollDetails.Sum(column => column.PPH21) + payrollDetails.Sum(column => column.ManagementFeeBilling) + payrollDetails.Sum(column => column.TakeHomePay) + payrollDetails.Sum(column => column.TaxBilling) + payrollDetails.Sum(column => column.AnotherDeduction) + (payrollDetails.Sum(column => column.PensionDeduction) + payrollDetails.Sum(column => column.BpjsKesehatanDeduction)) + (payrollDetails.Sum(column => column.BpjsBilling)));
                            await HideColumn(4);
                            await HideColumn(5);
                            await HideColumn(6);
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                            
                        }
                    }                    
                    excelPackage.Workbook.Worksheets.Delete(template);
                    MemoryStream stream = new MemoryStream();
                    excelPackage.SaveAs(stream);
                    byte[] content = stream.ToArray();
                    return File(content, "application/excel", $"Bank Data {allPayrollDetails.First().PayrollHistory.Month} {allPayrollDetails.First().PayrollHistory.Year}.xlsx");
                }
                return Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Download Report");
                throw error;
            }
        }

        [Route("PayrollHistory/Download/Report/{payrollHistoryId}/{districtId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReportDistrict(int payrollHistoryId, int districtId)
        {
            try
            {
                FileInfo excelTemplate = new FileInfo("wwwroot/file/templateReport.xlsx");
                List<PayrollDetail> allPayrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.Employee.Customer)
                    .Include(table => table.Employee.Location.District)
                    .Include(table => table.Employee.Position)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.PayrollHistoryId == payrollHistoryId)
                    .Where(column => column.Employee.Location.DistrictId == districtId)
                    .OrderBy(column => column.Employee.CustomerId)
                    .ToListAsync();
                if (allPayrollDetails.Count() == 0)
                {
                    return BadRequest("Tidak Ada Data");
                }

                List<Customer> customers = await payrollDB.Customer
                    .Where(column => allPayrollDetails.Select(col => col.Employee.CustomerId).Contains(column.Id))
                    .ToListAsync();
                List<Location> allLocations = await payrollDB.Location
                    .ToListAsync();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(excelTemplate.FullName)))
                {
                    ExcelWorkbook workbook = excelPackage.Workbook;
                    ExcelWorksheet template = workbook.Worksheets.FirstOrDefault();

                    foreach (Customer customer in customers)
                    {

                        List<Location> locations = allLocations
                          .Where(col => allPayrollDetails.Select(column => column.Employee.LocationId).Contains(col.Id))
                          .Where(col => allPayrollDetails.Select(column => column.Employee.CustomerId).Contains(customer.Id))
                          .ToList();
                        if (locations.Count == 0)
                        {
                            continue;
                        }
                        foreach (Location location in locations)
                        {

                            string worksheetName = $"{customer.Name} {location.Name}";
                            worksheet = workbook.Worksheets.Copy(template.ToString(), worksheetName);
                            await Merge($"A1", $"C1");
                            await SetValue($"A2", $"Tagihan Driver {location.Name} {customer.Name}");
                            await Merge($"A2", $"C2");
                            await SetValue($"A3", $"Periode {allPayrollDetails.First().PayrollHistory.Month} {allPayrollDetails.First().PayrollHistory.Year} ");
                            await Merge($"A3", $"C3");
                            List<PayrollDetail> payrollDetails = allPayrollDetails
                              .Where(col => col.Employee.CustomerId == customer.Id)
                              .Where(col => col.Employee.LocationId == location.Id)
                              .ToList();
                            int currentRow = 7;
                            int no = 1;
                            foreach (PayrollDetail payrollDetail in payrollDetails)
                            {
                                await SetValue($"A{currentRow}", no);
                                //TODO
                                //await SetValue($"B{currentRow}", payrollDetail.Employee.NIK);
                                await SetValue($"C{currentRow}", payrollDetail.Employee.Name);
                                await SetValue($"D{currentRow}", payrollDetail.Employee.Position.Name);
                                await SetValue($"E{currentRow}", payrollDetail.Employee.Location.Name);
                                await SetValue($"F{currentRow}", payrollDetail.Employee.Customer.Name);
                                await SetValue($"G{currentRow}", payrollDetail.MainSalaryBilling);
                                await SetValue($"H{currentRow}", payrollDetail.JamsostekBilling);
                                await SetValue($"I{currentRow}", payrollDetail.BpjsBilling);
                                await SetValue($"J{currentRow}", payrollDetail.PensionBilling);
                                await SetValue($"K{currentRow}", payrollDetail.AtributeBilling);
                                await SetValue($"L{currentRow}", payrollDetail.MainPrice);
                                await SetValue($"M{currentRow}", payrollDetail.ManagementFeeBilling);
                                await SetValue($"N{currentRow}", payrollDetail.InsentiveBilling);
                                await SetValue($"O{currentRow}", payrollDetail.AttendanceBilling);
                                await SetValue($"P{currentRow}", payrollDetail.AppreciationBilling);
                                await SetValue($"Q{currentRow}", payrollDetail.OvertimeBilling);
                                await SetValue($"R{currentRow}", payrollDetail.SubtotalBilling);
                                await SetValue($"S{currentRow}", payrollDetail.TaxBilling);
                                await SetValue($"T{currentRow}", payrollDetail.GrandTotalBilling);
                                //await SetValue($"U{currentRow}", payrollDetail.Employee.JoinCustomerDate);

                                await SetValue($"X{currentRow}", no);
                                //TODO
                                //await SetValue($"Y{currentRow}", payrollDetail.Employee.NIK);
                                await SetValue($"Z{currentRow}", payrollDetail.Employee.Name);
                                await SetValue($"AA{currentRow}", payrollDetail.Employee.FamilyStatusCode);
                                await SetValue($"AB{currentRow}", payrollDetail.ResultPayroll);
                                await SetValue($"AC{currentRow}", payrollDetail.Rapel);
                                await SetValue($"AD{currentRow}", payrollDetail.BpjsReturn);
                                await SetValue($"AE{currentRow}", payrollDetail.FeePayroll);
                                await SetValue($"AF{currentRow}", payrollDetail.TotalPayroll);
                                await SetValue($"AG{currentRow}", payrollDetail.TaxPayroll);
                                await SetValue($"AH{currentRow}", payrollDetail.GrossPayroll);
                                await SetValue($"AI{currentRow}", payrollDetail.AttributePayroll);
                                await SetValue($"AJ{currentRow}", payrollDetail.BpjsTkDeduction);
                                await SetValue($"AK{currentRow}", payrollDetail.BpjsKesehatanDeduction);
                                await SetValue($"AL{currentRow}", payrollDetail.PensionDeduction);
                                await SetValue($"AM{currentRow}", payrollDetail.PKP1);
                                await SetValue($"AN{currentRow}", payrollDetail.PTKP);
                                await SetValue($"AO{currentRow}", payrollDetail.PKP2);
                                await SetValue($"AP{currentRow}", payrollDetail.PPH21);
                                await SetValue($"AQ{currentRow}", payrollDetail.PPH23);
                                await SetValue($"AR{currentRow}", payrollDetail.Netto);
                                await SetValue($"AS{currentRow}", payrollDetail.AnotherDeduction);
                                await SetValue($"AT{currentRow}", payrollDetail.TakeHomePay);

                                await SetValue($"AV{currentRow}", no);
                                //TODO
                                //await SetValue($"AW{currentRow}", payrollDetail.Employee.NIK);
                                await SetValue($"AX{currentRow}", payrollDetail.Employee.Name);
                                await SetValue($"AY{currentRow}", payrollDetail.TakeHomePay);

                                currentRow++;
                                no++;
                            }

                            await SetValue($"A{currentRow}", $"Total");
                            await Merge($"A{currentRow}", $"F{currentRow}");
                            await SetValue($"G{currentRow}", payrollDetails.Sum(column => column.MainSalaryBilling));
                            await SetValue($"H{currentRow}", payrollDetails.Sum(column => column.JamsostekBilling));
                            await SetValue($"I{currentRow}", payrollDetails.Sum(column => column.BpjsBilling));
                            await SetValue($"J{currentRow}", payrollDetails.Sum(column => column.PensionBilling));
                            await SetValue($"K{currentRow}", payrollDetails.Sum(column => column.AtributeBilling));
                            await SetValue($"L{currentRow}", payrollDetails.Sum(column => column.MainPrice));
                            await SetValue($"M{currentRow}", payrollDetails.Sum(column => column.ManagementFeeBilling));
                            await SetValue($"N{currentRow}", payrollDetails.Sum(column => column.InsentiveBilling));
                            await SetValue($"O{currentRow}", payrollDetails.Sum(column => column.AttendanceBilling));
                            await SetValue($"P{currentRow}", payrollDetails.Sum(column => column.AppreciationBilling));
                            await SetValue($"Q{currentRow}", payrollDetails.Sum(column => column.OvertimeBilling));
                            await SetValue($"R{currentRow}", payrollDetails.Sum(column => column.SubtotalBilling));
                            await SetValue($"S{currentRow}", payrollDetails.Sum(column => column.TaxBilling));
                            await SetValue($"T{currentRow}", payrollDetails.Sum(column => column.GrandTotalBilling));

                            await SetValue($"AB{currentRow}", payrollDetails.Sum(column => column.ResultPayroll));
                            await SetValue($"AC{currentRow}", payrollDetails.Sum(column => column.Rapel));
                            await SetValue($"AD{currentRow}", payrollDetails.Sum(column => column.BpjsReturn));
                            await SetValue($"AE{currentRow}", payrollDetails.Sum(column => column.FeePayroll));
                            await SetValue($"AF{currentRow}", payrollDetails.Sum(column => column.TotalPayroll));
                            await SetValue($"AG{currentRow}", payrollDetails.Sum(column => column.TaxPayroll));
                            await SetValue($"AH{currentRow}", payrollDetails.Sum(column => column.GrossPayroll));
                            await SetValue($"AI{currentRow}", payrollDetails.Sum(column => column.AttributePayroll));
                            await SetValue($"AJ{currentRow}", payrollDetails.Sum(column => column.BpjsTkDeduction));
                            await SetValue($"AK{currentRow}", payrollDetails.Sum(column => column.BpjsKesehatanDeduction));
                            await SetValue($"AL{currentRow}", payrollDetails.Sum(column => column.PensionDeduction));
                            await SetValue($"AM{currentRow}", payrollDetails.Sum(column => column.PKP1));
                            await SetValue($"AN{currentRow}", payrollDetails.Sum(column => column.PTKP));
                            await SetValue($"AO{currentRow}", payrollDetails.Sum(column => column.PKP2));
                            await SetValue($"AP{currentRow}", payrollDetails.Sum(column => column.PPH21));
                            await SetValue($"AQ{currentRow}", payrollDetails.Sum(column => column.PPH23));
                            await SetValue($"AR{currentRow}", payrollDetails.Sum(column => column.Netto));
                            await SetValue($"AS{currentRow}", payrollDetails.Sum(column => column.AnotherDeduction));
                            await SetValue($"AT{currentRow}", payrollDetails.Sum(column => column.TakeHomePay));

                            await SetValue($"AY{currentRow}", payrollDetails.Sum(column => column.TakeHomePay));

                            await Borderize($"A6", $"V{currentRow}");
                            await Borderize($"X6", $"AT{currentRow}");
                            await Borderize($"AV6", $"AY{currentRow}");

                            currentRow = currentRow + 2;
                            await SetValue($"AB{currentRow}", $"BPJS TK");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.JamsostekBilling)) + (payrollDetails.Sum(column => column.PensionBilling)) + (payrollDetails.Sum(column => column.BpjsTkDeduction)) + (payrollDetails.Sum(column => column.PensionDeduction)));

                            await SetValue($"AB{currentRow}", $"BPJS Kes");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.BpjsKesehatanDeduction)) + (payrollDetails.Sum(column => column.BpjsBilling)));

                            await SetValue($"AB{currentRow}", $"Perlengkapan");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.AtributeBilling)));

                            await SetValue($"AB{currentRow}", $"PPH 21");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.PPH21)));

                            await SetValue($"AB{currentRow}", $"Potongan");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.AnotherDeduction)));

                            await SetValue($"AB{currentRow}", $"Fee");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.ManagementFeeBilling)));

                            await SetValue($"AB{currentRow}", $"PPN");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.TaxBilling)));

                            await SetValue($"AB{currentRow}", $"TF");
                            await SetValue($"AC{currentRow++}", (payrollDetails.Sum(column => column.TakeHomePay)));

                            await SetValue($"AB{currentRow}", $"Total");
                            await SetValue($"AC{currentRow++}", payrollDetails.Sum(column => column.TakeHomePay) + payrollDetails.Sum(column => column.AtributeBilling) + payrollDetails.Sum(column => column.JamsostekBilling) + (payrollDetails.Sum(column => column.PensionBilling)) + (payrollDetails.Sum(column => column.BpjsTkDeduction)) + payrollDetails.Sum(column => column.PPH21) + payrollDetails.Sum(column => column.ManagementFeeBilling) + payrollDetails.Sum(column => column.TakeHomePay) + payrollDetails.Sum(column => column.TaxBilling) + payrollDetails.Sum(column => column.AnotherDeduction) + (payrollDetails.Sum(column => column.PensionDeduction) + payrollDetails.Sum(column => column.BpjsKesehatanDeduction)) + (payrollDetails.Sum(column => column.BpjsBilling)));
                            await HideColumn(4);
                            await HideColumn(5);
                            await HideColumn(6);
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        }
                    }
                    excelPackage.Workbook.Worksheets.Delete(template);
                    MemoryStream stream = new MemoryStream();
                    excelPackage.SaveAs(stream);
                    byte[] content = stream.ToArray();
                    return File(content, "application/excel", $"Bank Data {allPayrollDetails.First().Employee.Location.District.Name} {allPayrollDetails.First().PayrollHistory.Month} {allPayrollDetails.First().PayrollHistory.Year}.xlsx");
                }
                return Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Download Report");
                throw error;
            }
        }


        private async Task<IActionResult> Borderize(string start, string end)
        {
            try
            { 
                ExcelRange modelTable = worksheet.Cells[$"{start}:{end}"];
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Borderize");
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

        private async Task<IActionResult> Merge(string start, string end)
        {
            try
            {
                worksheet.Cells[$"{start}:{end}"].Merge = true;
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Merge");
                throw error;
            }
        }

        private async Task<IActionResult> HideColumn(int cell)
        {
            try
            {
                worksheet.Column(cell).Hidden = true;
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll History Controller - Hide Column");
                throw error;
            }
        }
    }
}
