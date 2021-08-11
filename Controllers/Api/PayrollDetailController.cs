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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class PayrollDetailController : ControllerBase
    {
        private readonly ILogger<PayrollDetailController> logger;
        private readonly PayrollDB payrollDB;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PayrollDetailController(ILogger<PayrollDetailController> _logger, PayrollDB _payrollDB, IHttpContextAccessor _httpContextAccessor)
        {
            payrollDB = _payrollDB;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        [Route("api/payrollDetail/read/{id}")]
        public async Task<IActionResult> Read(int id)
        {
            try
            {
                List<PayrollDetail> payrollDetail = await payrollDB.PayrollDetail
                    .Include(table => table.PayrollHistory)
                    .Include(table => table.Employee.Location)
                    .Where(column => column.PayrollHistoryId == id)
                    .ToListAsync();
                return new JsonResult(payrollDetail);
            } catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Read {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/readDatatable/{id}")]
        public async Task<IActionResult> ReadDatatable(int id)
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                PayrollDetailView payrollDetailView = new PayrollDetailView();
                payrollDetailView.Data = await payrollDB.PayrollDetail
                    .Include(table => table.Employee.Location)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.PayrollHistoryId == id)
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Employee.Name.Contains(request.Keyword))
                    .OrderBy(column => column.PayrollDetailStatusId)
                    .Skip(request.Skip)
                    .Take(request.PageSize)
                    .ToListAsync();
                payrollDetailView.RecordsFiltered = await payrollDB.PayrollDetail
                        .Where(column => column.IsExist == true)
                        .Where(column => column.PayrollHistoryId == id)
                        .CountAsync();
                return new JsonResult(payrollDetailView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Read Datatable");
                throw error;
            }        
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/readPersonalDatatable")]
        public async Task<IActionResult> ReadPersonalDatatable()
        {
            try
            {
                int userNIK = httpContextAccessor.HttpContext.User.GetNIK();
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                PayrollDetailView payrollDetailView = new PayrollDetailView();
                payrollDetailView.Data = await payrollDB.PayrollDetail
                    .Include(table => table.Employee.Location)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.Employee.NIK == userNIK)
                    .Where(column => column.IsExist == true)
                    .Where(column => column.PayrollDetailStatusId == 3)
                    .OrderBy(column => column.Id)                    
                    .Skip(request.Skip)
                    .Take(request.PageSize)
                    .ToListAsync();
                payrollDetailView.RecordsFiltered = await payrollDB.PayrollDetail
                    .Include(table => table.Employee)
                    .Where(column => column.Employee.NIK == userNIK)
                    .Where(column => column.PayrollDetailStatusId == 3)
                    .CountAsync();
                return new JsonResult(payrollDetailView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Read Personal Datatable");
                throw error;
            }
        }


        [Authorize]
        [HttpGet]
        [Route("api/payrollDetail/readDetail/{id}")]
        public async Task<IActionResult> ReadDetail(int id)
        {
            try
            {
                PayrollDetail payrollDetail = await payrollDB.PayrollDetail
                    .Include(table => table.PayrollHistory)
                    .Include(table => table.Employee)
                    .Include(table => table.Employee.Location.District)
                    .Include(table => table.Employee.FamilyStatus)
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                return new JsonResult(payrollDetail);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Read Detail {id}");
                throw error;
            }
        }

        //[Authorize]
        //[HttpPost]
        //[Route("api/payrollDetail/update/{id}")]
        //public async Task<IActionResult>Update(int id, IFormFile file)
        //{
        //    try
        //    {
        //        MasterData masterData = new MasterData();
        //        masterData.payrollDetails = await payrollDB.PayrollDetail
        //            .Include(table => table.PayrollHistory)
        //            .Include(table => table.Employee.Location.District)
        //            .Include(table => table.Employee.FamilyStatus)
        //            .Where(column => column.PayrollHistoryId == id)
        //            .ToListAsync();
        //        masterData.PayrollHistory = await payrollDB.PayrollHistory
        //            .Where(column => column.Id == id)
        //            .FirstOrDefaultAsync();
        //        masterData.Employees = await payrollDB.Employee
        //            .Where(column => column.IsExist == true)
        //            .ToListAsync();
        //        using (var stream = new MemoryStream())
        //        {
        //            file.CopyTo(stream);
        //            stream.Position = 0;
        //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //            using (ExcelPackage excelPackage = new ExcelPackage(stream))
        //            {
        //                bool isError = false;
        //                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
        //                {
        //                    WorksheetResult worksheetResult = await ReadExcel(worksheet, masterData);
        //                    if (worksheetResult.Worksheet != null)
        //                    {
        //                        if (worksheetResult.IsError && worksheetResult.IsAcceptableFormat)
        //                        {
        //                            isError = true;
        //                            string sheetName = $"{worksheetResult.Worksheet.Name}_REV";
        //                            excelPackage.Workbook.Worksheets.Add(sheetName, worksheetResult.Worksheet);
        //                        }
        //                        excelPackage.Workbook.Worksheets.Delete(worksheet);

        //                    }
        //                }
        //                if (isError)
        //                {
        //                    string excelFileDirectory = $"wwwroot/file/payrolldetail.xlsx";
        //                    FileInfo excelFile = new FileInfo(excelFileDirectory);
        //                    await excelPackage.SaveAsAsync(excelFile);
        //                    return BadRequest(excelFile.FullName.ToString());
        //                }

        //            }
        //        }
        //        return new JsonResult(Ok());
        //    }
        //    catch (Exception error)
        //    {
        //        logger.LogError(error, $"Payroll Detail API - Update {id}");
        //        throw;
        //    }
        //}

        //private async Task<WorksheetResult> ReadExcel(ExcelWorksheet worksheet, MasterData masterData)
        //{
        //    WorksheetResult worksheetResult = new WorksheetResult();
        //    worksheetResult.LastRow = 0;
        //    List<Employee> SuccessBillingEmployee = new List<Employee>();
        //    List<PayrollDetail> UpdatedPayroll = new List<PayrollDetail>();
        //    try
        //    {
        //        MappingExcelPayrollDetail mapping = new MappingExcelPayrollDetail(worksheet);
        //        if (worksheetResult.IsAcceptableFormat = mapping.IsAcceptable)
        //        {
        //            for (int currentRow = mapping.InRowStart; currentRow <= mapping.InRowEnd; currentRow++)
        //            {
        //                worksheetResult.LastRow = currentRow;
        //                bool isAny;
        //                PayrollDetail payrollDetail = new PayrollDetail();

        //                ExcelIntreader nik = new ExcelIntreader(worksheet, $"{mapping.NIK}{currentRow}");
        //                if (nik.IsExist && nik.IsInteger)
        //                {
        //                    bool isAnyEmployee = masterData.Employees
        //                        .Where(column => column.NIK == nik.Value)
        //                        .Any();
        //                    if (isAnyEmployee)
        //                    {
        //                        isAny = masterData.payrollDetails
        //                            .Where(column => column.Employee.NIK == nik.Value)
        //                            .Where(column => column.PayrollHistoryId == masterData.PayrollHistory.Id)
        //                            .Any();
        //                        if (isAny)
        //                        {
        //                            payrollDetail = masterData.payrollDetails
        //                                .Where(column => column.Employee.NIK == nik.Value)
        //                                .Where(column => column.PayrollHistoryId == masterData.PayrollHistory.Id)
        //                                .FirstOrDefault();
        //                        }
        //                        else
        //                        {
        //                            continue;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        continue;
        //                    }
        //                }
        //                else
        //                {
        //                    worksheet.Cells[$"{mapping.NIK}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    worksheet.Cells[$"{mapping.NIK}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
        //                    continue;
        //                }

        //                //NAME
        //                ExcelStringReader name = new ExcelStringReader(worksheet, $"{mapping.Name}{currentRow}");
        //                if (name.IsExist)
        //                {
        //                    bool isAnyName = masterData.Employees
        //                        .Where(column => column.NIK == nik.Value)
        //                        .Where(column => column.Name == name.Value)
        //                        .Any();                            
        //                }
        //                else
        //                {
        //                    worksheet.Cells[$"{mapping.Name}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    worksheet.Cells[$"{mapping.Name}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
        //                    continue;
        //                }

        //                payrollDetail.MainSalaryBilling = GetIntValue(worksheet, mapping.MainSalaryBilling, currentRow);
        //                payrollDetail.JamsostekBilling = GetIntValue(worksheet, mapping.JamsostekBilling, currentRow);
        //                payrollDetail.BpjsBilling = GetIntValue(worksheet, mapping.BpjsBilling, currentRow);
        //                payrollDetail.PensionBilling = GetIntValue(worksheet, mapping.PensionBilling, currentRow);
        //                payrollDetail.AtributeBilling = GetIntValue(worksheet, mapping.AtributeBilling, currentRow);
        //                payrollDetail.MainPrice = GetIntValue(worksheet, mapping.MainPrice, currentRow);
        //                payrollDetail.ManagementFeeBilling = GetIntValue(worksheet, mapping.ManagementFeeBilling, currentRow);
        //                payrollDetail.InsentiveBilling = GetIntValue(worksheet, mapping.InsentiveBilling, currentRow);
        //                payrollDetail.AttendanceBilling = GetIntValue(worksheet, mapping.AttendanceBilling, currentRow);
        //                if (mapping.AppreciationBilling!= null)
        //                {
        //                    payrollDetail.AppreciationBilling = GetIntValue(worksheet, mapping.AppreciationBilling, currentRow);
        //                }
        //                payrollDetail.OvertimeBilling = GetIntValue(worksheet, mapping.OvertimeBilling, currentRow);
        //                payrollDetail.SubtotalBilling = GetIntValue(worksheet, mapping.SubtotalBilling, currentRow);
        //                payrollDetail.TaxBilling = GetIntValue(worksheet, mapping.TaxBilling, currentRow);
        //                payrollDetail.GrandTotalBilling = GetIntValue(worksheet, mapping.GrandTotalBilling, currentRow);
        //                if (mapping.AnotherDeduction != null)
        //                {
        //                    payrollDetail.AnotherDeduction = GetIntValue(worksheet, "W", currentRow);
        //                }

        //                if (payrollDetail.IsValidGrandTotalBilling)
        //                {
        //                    payrollDetail.PayrollDetailStatusId = 2;
        //                    payrollDetail.ResultPayroll = Convert.ToInt32(payrollDetail.MainSalaryBilling + payrollDetail.InsentiveBilling + payrollDetail.AttendanceBilling + payrollDetail.OvertimeBilling + payrollDetail.AppreciationBilling);
        //                    payrollDetail.FeePayroll = Convert.ToInt32(payrollDetail.ManagementFeeBilling);
        //                    payrollDetail.TotalPayroll = Convert.ToInt32(payrollDetail.FeePayroll + payrollDetail.ResultPayroll);
        //                    payrollDetail.TaxPayroll = Convert.ToInt32((payrollDetail.FeePayroll * masterData.PayrollHistory.PpnPercentage) / 100);
        //                    payrollDetail.GrossPayroll = Convert.ToInt32(payrollDetail.TotalPayroll + payrollDetail.TaxPayroll);
        //                    payrollDetail.AttributePayroll = Convert.ToInt32(payrollDetail.AtributeBilling);
        //                    payrollDetail.BpjsTkDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * masterData.PayrollHistory.BpjsTk1Percentage) / 100);
        //                    if (payrollDetail.Employee.BpjsRemark != null)
        //                    {
        //                        if (payrollDetail.Employee.BpjsRemark.ToLower().Replace(" ", string.Empty) != "bumbk")
        //                        {
        //                            payrollDetail.BpjsKesehatanDeduction = 0;
        //                            payrollDetail.BpjsReturn = payrollDetail.BpjsKesehatanDeduction;
        //                        }
        //                        else
        //                        {
        //                            payrollDetail.BpjsKesehatanDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * masterData.PayrollHistory.BpjsPayrollPercentage) / 100);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        payrollDetail.BpjsKesehatanDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * masterData.PayrollHistory.BpjsPayrollPercentage) / 100);
        //                    }

        //                    payrollDetail.PensionDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * masterData.PayrollHistory.PensionPayrollPercentage) / 100);
        //                    payrollDetail.PTKP = Convert.ToInt32(payrollDetail.Employee.FamilyStatus.PTKP);
        //                    payrollDetail.PKP1 = Convert.ToInt32(payrollDetail.ResultPayroll - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction); ;
        //                    payrollDetail.PKP2 = Convert.ToInt32(payrollDetail.PKP1 - payrollDetail.PTKP);
        //                    if (payrollDetail.PKP2 > 1)
        //                    {
        //                        payrollDetail.PPH21 = Convert.ToInt32((payrollDetail.PKP2 * payrollDetail.PayrollHistory.Pph21Percentage) / 100);
        //                    }

        //                    if (payrollDetail.Employee.BankCode != "BCA")
        //                    {
        //                        payrollDetail.TransferFee = 6500;
        //                    }

        //                    payrollDetail.PPH23 = Convert.ToInt32((payrollDetail.FeePayroll * payrollDetail.PayrollHistory.Pph23Percentage) / 100);
        //                    payrollDetail.Netto = Convert.ToInt32(payrollDetail.ResultPayroll + payrollDetail.Rapel + payrollDetail.BpjsReturn - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.PPH21);
        //                    payrollDetail.TakeHomePay = Convert.ToInt32(payrollDetail.Netto - payrollDetail.AnotherDeduction - payrollDetail.TransferFee);
        //                    payrollDB.Entry(payrollDetail).State = EntityState.Modified;
        //                    SuccessBillingEmployee.Add(payrollDetail.Employee);
        //                    UpdatedPayroll.Add(payrollDetail);
        //                }


        //            }
        //            payrollDB.PayrollDetail.UpdateRange(UpdatedPayroll);
        //            await payrollDB.SaveChangesAsync();
        //        }
        //        worksheetResult.IsError = false;
        //    }
        //    catch (Exception error)
        //    {
        //        logger.LogError(error, $"Read Excel {worksheet.Name} row {worksheetResult.LastRow}");
        //        worksheetResult.ErrorMessage = error.Message.ToString();
        //        worksheetResult.IsError = true;
        //    }
        //    return worksheetResult;
        //}

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/resync/{id}")]
        public async Task<IActionResult> Resync(int id)
        {
            try
            {
                //Add new employees
                List<PayrollDetail> newPayrollDetails = new List<PayrollDetail>();
                List<PayrollDetail> oldPayrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.PayrollHistory)
                    .Include(table => table.Employee.Location)
                    .Include(table => table.Employee.FamilyStatus)
                    .Where(column => column.PayrollHistoryId == id)
                    .ToListAsync();
                List<PayrollDetail> updatedPayrollDetails = new List<PayrollDetail>();
                PayrollHistory payrollHistory = payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                List<Employee> employees = await payrollDB.Employee
                    .Include(table => table.Location)
                    .Where(column => column.IsExist == true)
                    .ToListAsync();
                List<Employee> oldEmployees = await payrollDB.PayrollDetail
                    .Include(table => table.Employee)
                    .Where(column => column.PayrollHistoryId == id)
                    .Select(column => column.Employee)
                    .ToListAsync();
                List<Employee> newEmployees = employees.Except(oldEmployees)
                    .ToList();
                foreach (Employee employee in newEmployees)
                {
                    PayrollDetail payrollDetail = new PayrollDetail();
                    payrollDetail.EmployeeId = employee.NIK;
                    payrollDetail.PayrollHistoryId = payrollHistory.Id;
                    payrollDetail.MainPrice = 0;
                    payrollDetail.PayrollDetailStatusId = 1;
                    payrollDB.Entry(payrollDetail).State = EntityState.Added;
                    newPayrollDetails.Add(payrollDetail);
                }
                await payrollDB.PayrollDetail.AddRangeAsync(newPayrollDetails);
                await payrollDB.SaveChangesAsync();

                //recalculate old employee
                foreach (PayrollDetail payrollDetail in oldPayrollDetails)
                {
                    if (payrollDetail.MainSalaryBilling != 0)
                    {
                        payrollDetail.PayrollDetailStatusId = 2;
                        payrollDetail.ResultPayroll = Convert.ToInt32(payrollDetail.MainSalaryBilling + payrollDetail.InsentiveBilling + payrollDetail.AttendanceBilling + payrollDetail.OvertimeBilling + payrollDetail.AppreciationBilling);
                        payrollDetail.FeePayroll = Convert.ToInt32(payrollDetail.ManagementFeeBilling);
                        payrollDetail.TotalPayroll = Convert.ToInt32(payrollDetail.FeePayroll + payrollDetail.ResultPayroll);
                        payrollDetail.TaxPayroll = Convert.ToInt32((payrollDetail.FeePayroll * payrollHistory.PpnPercentage) / 100);
                        payrollDetail.GrossPayroll = Convert.ToInt32(payrollDetail.TotalPayroll + payrollDetail.TaxPayroll);
                        payrollDetail.AttributePayroll = Convert.ToInt32(payrollDetail.AtributeBilling);
                        payrollDetail.BpjsTkDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsTk1Percentage) / 100);
                        if (payrollDetail.Employee.BpjsRemark != null)
                        {
                            if (payrollDetail.Employee.BpjsRemark.ToLower().Replace(" ", string.Empty) != "bumbk")
                            {
                                payrollDetail.BpjsKesehatanDeduction = 0;
                            }
                            else
                            {
                                payrollDetail.BpjsKesehatanDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsPayrollPercentage) / 100);
                            }
                        }
                        else
                        {
                            payrollDetail.BpjsKesehatanDeduction = 0;
                        }

                        payrollDetail.BpjsReturn = 0;
                        payrollDetail.PensionDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.PensionPayrollPercentage) / 100);
                        payrollDetail.PTKP = Convert.ToInt32(payrollDetail.Employee.FamilyStatus.PTKP);
                        payrollDetail.PKP1 = Convert.ToInt32(payrollDetail.ResultPayroll - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.AnotherDeduction) ;
                        payrollDetail.PKP2 = Convert.ToInt32(payrollDetail.PKP1 - payrollDetail.PTKP);
                        if (payrollDetail.PKP2 > 1)
                        {
                            payrollDetail.PPH21 = Convert.ToInt32((payrollDetail.PKP2 * payrollDetail.PayrollHistory.Pph21Percentage) / 100);
                        }
                        else
                        {
                            payrollDetail.PPH21 = 0;

                        }

                        //if (payrollDetail.Employee.BankCode != "BCA")
                        //{
                        //    payrollDetail.TransferFee = 6500;
                        //}

                        payrollDetail.PPH23 = Convert.ToInt32((payrollDetail.FeePayroll * payrollDetail.PayrollHistory.Pph23Percentage) / 100);
                        payrollDetail.Netto = Convert.ToInt32(payrollDetail.ResultPayroll + payrollDetail.Rapel + payrollDetail.BpjsReturn - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.PPH21);
                        payrollDetail.TakeHomePay = Convert.ToInt32(payrollDetail.Netto - payrollDetail.AnotherDeduction - payrollDetail.TransferFee);
                        payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                        updatedPayrollDetails.Add(payrollDetail);
                    }
                }

                payrollDB.PayrollDetail.UpdateRange(updatedPayrollDetails);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Payroll Detail API - Resync    {id}");
                return BadRequest(error.Message);
            }
        }



        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/update/{id}")]
        public async Task<IActionResult> Update(int id, IFormFile file)
        {
            try
            {
                //Check if ID  exist
                if (id == 0)
                {
                    return BadRequest($"Gagal mendapatkan Id, silahkan ulangi kembali");
                }

                //Check if File valid
                string extension = Path.GetExtension(file.FileName);
                if (extension != ".xlsx" && extension != ".xls")
                {
                    return BadRequest($"formatt file {extension} tidak diijinkan, silahkan upload file dengan format excel");
                }

                //List Up Needed Data
                List<PayrollDetail> payrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.PayrollHistory)
                    .Include(table => table.Employee.Location.District)
                    .Include(table => table.Employee.FamilyStatus)
                    .Where(column => column.PayrollHistoryId == id)
                    .ToListAsync();
                PayrollHistory payrollHistory = await payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                List<Employee> employees = await payrollDB.Employee
                    .Where(column => column.IsExist == true)
                    .ToListAsync();

                //Access File
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Copy file to memory stream
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
                    {
                        if (excelPackage.Workbook.Worksheets.Count == 0)
                        {
                            return BadRequest($"Tidak ada worksheet yang tersedia pada file {file.FileName}");
                        }

                        foreach (ExcelWorksheet excelWorksheet in excelPackage.Workbook.Worksheets)
                        {
                            AddressPayroll address = new AddressPayroll(excelWorksheet);       
                        }
                    }

                }
                return Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Update {id}");
                return BadRequest(error.Message);
                throw error;
            }

        }


        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/update1/{id}")]
        public async Task<IActionResult> Update1(int id, IFormFile file)
        {
            int startRow = 0;
            int endRow = 0;
            string currentWorksheet = null;
            List<Employee> FailedBillingEmployee = new List<Employee>();
            List<Employee> SuccessBillingEmployee = new List<Employee>();
            try
            {
                List<PayrollDetail> payrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.PayrollHistory)
                    .Include(table => table.Employee.Location.District)
                    .Include(table => table.Employee.FamilyStatus)
                    .Where(column => column.PayrollHistoryId == id)
                    .ToListAsync();
                PayrollHistory payrollHistory = await payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                List<Employee> employees = await payrollDB.Employee
                    .Where(column => column.IsExist == true)
                    .ToListAsync();
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var excelPackage = new ExcelPackage(stream))
                    {
                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        {
                            currentWorksheet = worksheet.Name;
                            startRow = 0;
                            endRow = 0;
                            bool isSetEndRow = false;
                            for (int currentRow = worksheet.Dimension.End.Row; currentRow > 0; currentRow--)
                            {
                                if (GetStringValue(worksheet, "A", currentRow) != "" && GetStringValue(worksheet, "A", currentRow) != "no" && GetStringValue(worksheet, "A", currentRow + 1) == "" && !isSetEndRow)
                                {
                                    isSetEndRow = true;
                                    endRow = currentRow;
                                }
                                if (GetStringValue(worksheet, "A", currentRow) == "no")
                                {
                                    startRow = currentRow + 1;
                                }
                            }

                            for (int currentRow = startRow; currentRow <= endRow; currentRow++)
                            {
                                int employeeId = GetIntValue(worksheet, "B", currentRow);
                                bool isExist = payrollDetails
                                    .Where(column => column.EmployeeId == employeeId)
                                    .Any();
                                if (isExist)
                                {
                                    bool isAnyEmployee = employees
                                        .Where(column => column.NIK == employeeId)
                                        .Any();
                                    Employee employee = employees
                                        .Where(column => column.NIK == employeeId)
                                        .FirstOrDefault();
                                    bool isValidEmployee = employee.Name.ToLower().Replace(" ", string.Empty) == GetStringValue(worksheet, "C", currentRow);
                                    bool isExistEmployee = employee.IsExist == true;
                                    if ((isValidEmployee || isAnyEmployee) & isExistEmployee)
                                    {
                                        PayrollDetail payrollDetail = payrollDetails
                                            .Where(column => column.EmployeeId == employeeId)
                                            .FirstOrDefault();

                                        payrollDetail.MainSalaryBilling = GetIntValue(worksheet, "G", currentRow);
                                        payrollDetail.JamsostekBilling = GetIntValue(worksheet, "H", currentRow);
                                        payrollDetail.BpjsBilling = GetIntValue(worksheet, "I", currentRow);
                                        payrollDetail.PensionBilling = GetIntValue(worksheet, "J", currentRow);
                                        payrollDetail.AtributeBilling = GetIntValue(worksheet, "K", currentRow);
                                        payrollDetail.MainPrice = GetIntValue(worksheet, "L", currentRow);
                                        payrollDetail.ManagementFeeBilling = GetIntValue(worksheet, "M", currentRow);
                                        payrollDetail.InsentiveBilling = GetIntValue(worksheet, "N", currentRow);
                                        payrollDetail.AttendanceBilling = GetIntValue(worksheet, "O", currentRow);
                                        payrollDetail.AppreciationBilling = GetIntValue(worksheet, "P", currentRow);
                                        payrollDetail.OvertimeBilling = GetIntValue(worksheet, "Q", currentRow);
                                        payrollDetail.SubtotalBilling = GetIntValue(worksheet, "R", currentRow);
                                        payrollDetail.TaxBilling = GetIntValue(worksheet, "S", currentRow);
                                        payrollDetail.GrandTotalBilling = GetIntValue(worksheet, "T", currentRow);
                                        payrollDetail.AnotherDeduction = GetIntValue(worksheet, "W", currentRow);

                                        payrollDetail.PayrollDetailStatusId = 2;
                                        payrollDetail.ResultPayroll = Convert.ToInt32(payrollDetail.MainSalaryBilling + payrollDetail.InsentiveBilling + payrollDetail.AttendanceBilling + payrollDetail.OvertimeBilling + payrollDetail.AppreciationBilling);
                                        payrollDetail.FeePayroll = Convert.ToInt32(payrollDetail.ManagementFeeBilling);
                                        payrollDetail.TotalPayroll = Convert.ToInt32(payrollDetail.FeePayroll + payrollDetail.ResultPayroll);
                                        payrollDetail.TaxPayroll = Convert.ToInt32((payrollDetail.FeePayroll * payrollHistory.PpnPercentage) / 100);
                                        payrollDetail.GrossPayroll = Convert.ToInt32(payrollDetail.TotalPayroll + payrollDetail.TaxPayroll);
                                        payrollDetail.AttributePayroll = Convert.ToInt32(payrollDetail.AtributeBilling);
                                        payrollDetail.BpjsTkDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsTk1Percentage) / 100);
                                        if (payrollDetail.Employee.BpjsRemark != null)
                                        {
                                            if (payrollDetail.Employee.BpjsRemark.ToLower().Replace(" ", string.Empty) != "bumbk")
                                            {
                                                payrollDetail.BpjsKesehatanDeduction = 0;
                                                payrollDetail.BpjsReturn = payrollDetail.BpjsKesehatanDeduction;
                                            }
                                            else
                                            {
                                                payrollDetail.BpjsKesehatanDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsPayrollPercentage) / 100);
                                            }
                                        }
                                        else
                                        {
                                            payrollDetail.BpjsKesehatanDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsPayrollPercentage) / 100);
                                        }

                                        payrollDetail.PensionDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.PensionPayrollPercentage) / 100);
                                        payrollDetail.PTKP = Convert.ToInt32(payrollDetail.Employee.FamilyStatus.PTKP);
                                        payrollDetail.PKP1 = Convert.ToInt32(payrollDetail.ResultPayroll - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction); ;
                                        payrollDetail.PKP2 = Convert.ToInt32(payrollDetail.PKP1 - payrollDetail.PTKP);
                                        if (payrollDetail.PKP2 > 1)
                                        {
                                            payrollDetail.PPH21 = Convert.ToInt32((payrollDetail.PKP2 * payrollDetail.PayrollHistory.Pph21Percentage) / 100);
                                        }

                                        if (payrollDetail.Employee.BankCode != "BCA")
                                        {
                                            payrollDetail.TransferFee = 6500;
                                        }

                                        payrollDetail.PPH23 = Convert.ToInt32((payrollDetail.FeePayroll * payrollDetail.PayrollHistory.Pph23Percentage) / 100);
                                        payrollDetail.Netto = Convert.ToInt32(payrollDetail.ResultPayroll + payrollDetail.Rapel + payrollDetail.BpjsReturn - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.PPH21);
                                        payrollDetail.TakeHomePay = Convert.ToInt32(payrollDetail.Netto - payrollDetail.AnotherDeduction - payrollDetail.TransferFee);
                                        payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                                    }
                                    SuccessBillingEmployee.Add(employee);
                                }
                            }
                            payrollDB.PayrollDetail.UpdateRange(payrollDetails.Where(column => column.IsExist));
                            await payrollDB.SaveChangesAsync();
                            bool isAnyUnUpdated = payrollDetails
                                .Where(column => column.PayrollDetailStatusId == 1)
                                .Where(column => column.IsExist == true)
                                .Any();
                            if (!isAnyUnUpdated)
                            {
                                payrollHistory.StatusId = 2;
                                payrollDB.Entry(payrollHistory).State = EntityState.Modified;
                                payrollDB.PayrollHistory.Update(payrollHistory);
                                await payrollDB.SaveChangesAsync();
                            }

                            if (FailedBillingEmployee.Count() > 0)
                            {
                                return BadRequest(FailedBillingEmployee.Select(column => new { column.Name, column.NIK }));
                            }
                        }
                    }
                }
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Update {currentWorksheet} - {id}");
                return BadRequest(error.Message);
            }

        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                PayrollDetail payrollDetail = payrollDB.PayrollDetail
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                if (httpContextAccessor.HttpContext.User.GetRole() == "Admin")
                {
                    payrollDetail.IsExist = false;
                    payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                    payrollDB.PayrollDetail.Update(payrollDetail);
                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(Ok());
                }
                else 
                {
                    return BadRequest("Anda tidak memiliki hak akses");
                }

            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Detail API - Delete Detail");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/updateDetail/{id}")]
        public async Task<IActionResult> UpdateDetail([FromForm]PayrollDetailInput payrollDetailInput, int id)
        {
            try
            {
                PayrollDetail payrollDetail = payrollDB.PayrollDetail
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                if (payrollDetail.PayrollDetailStatusId == 1)
                {
                    return BadRequest("Silahkan upload terlebih dahulu");
                }
                else if (payrollDetail.PayrollDetailStatusId == 2)
                {
                    payrollDetail.Rapel = payrollDetailInput.Rapel;
                    payrollDetail.AnotherDeduction = payrollDetailInput.AnotherDeduction;
                    payrollDetail.TransferFee = payrollDetailInput.TransferFee;
                    payrollDetail.Netto = Convert.ToInt32(payrollDetail.ResultPayroll + payrollDetail.Rapel + payrollDetail.BpjsReturn - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.PPH21);
                    payrollDetail.TakeHomePay = Convert.ToInt32(payrollDetail.Netto - payrollDetail.AnotherDeduction - payrollDetail.TransferFee);
                    payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                    payrollDB.PayrollDetail.Update(payrollDetail);
                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(Ok());
                }
                else
                {
                    return BadRequest("Data yang sudah di submit tidak boleh dirubah");

                }
            }
            catch (Exception error)
            {
                logger.LogError(error, "Payroll Detail API - Update Detail");
                throw error;
            }
        }

        private string GetStringValue(ExcelWorksheet excelWorksheet, string column, int row)
        {
            try
            {
                string stringResult = null;
                if (excelWorksheet.Cells[$"{column}{row}"].Value != null)
                {
                    stringResult = excelWorksheet.Cells[$"{column}{row}"].Value.ToString().ToLower().Replace(" ", string.Empty);
                }
                else
                {
                    stringResult = "";
                }
                return stringResult;
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Get Int Value {row}");
                throw error;
            }
        }

        private decimal GetDecimalValue(ExcelWorksheet excelWorksheet, string column, int row)
        {
            try
            {
                decimal decimalResult = 0;
                if (excelWorksheet.Cells[$"{column}{row}"].Value != null)
                {
                    decimalResult = decimal.Parse(excelWorksheet.Cells[$"{column}{row}"].Value.ToString());
                }
                else
                {
                    decimalResult = 0;
                }
                return decimalResult;
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Get decimal Value {row}");
                throw error;
            }
        }

        private int GetIntValue(ExcelWorksheet excelWorksheet, string column, int row)
        {
            try
            {
                int intResult = 0;
                if (excelWorksheet.Cells[$"{column}{row}"].Value != null)
                {
                    var decimalValue = GetDecimalValue(excelWorksheet, column, row);
                    intResult = Convert.ToInt32(decimalValue);
                }
                return intResult;
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Get String Value {row}");
                throw error;
            }
        }


    }
}
