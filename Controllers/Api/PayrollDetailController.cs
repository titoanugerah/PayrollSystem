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
                return BadRequest(error.Message);
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
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/payrollDetail/readPersonalDatatable")]
        [Authorize]
        public async Task<IActionResult> ReadPersonalDatatable()
        {
            try
            {
                string userNIK = httpContextAccessor.HttpContext.User.GetNIK();
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
                return BadRequest(error.Message);
            }
        }


        [HttpGet]
        [Route("api/payrollDetail/readDetail/{id}")]
        [Authorize]
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
                return BadRequest(error.Message);
            }
        }

        [Authorize(Roles = "Admin")]
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
                PayrollHistory payrollHistory = payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                List<Employee> employees = await payrollDB.Employee
                    .Include(table => table.Location)
                    .Where(column => column.MainCustomerId == payrollHistory.MainCustomerId)
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
                //Assa
                if (payrollHistory.MainCustomerId == 1)
                {
                    await CalculateAssa(oldPayrollDetails, payrollHistory);
                }

                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Payroll Detail API - Resync    {id}");
                return BadRequest(error.Message);
            }
        }

        private async Task<IActionResult> CalculateAssa(List<PayrollDetail> payrollDetails, PayrollHistory payrollHistory)
        {
            try
            {
                List<PayrollDetail> updatedPayrollDetails = new List<PayrollDetail>();
                foreach (PayrollDetail payrollDetail in payrollDetails)
                {
                    if (payrollDetail.MainSalaryBilling != 0)
                    {
                        payrollDetail.PayrollDetailStatusId = 2;
                        payrollDetail.ResultPayroll = Convert.ToInt32(payrollDetail.MainSalaryBilling - payrollDetail.AbsentDeduction + payrollDetail.InsentiveBilling + payrollDetail.AttendanceBilling + payrollDetail.OvertimeBilling + payrollDetail.AppreciationBilling + payrollDetail.PulseAllowance);
                        payrollDetail.FeePayroll = Convert.ToInt32(payrollDetail.ManagementFeeBilling);
                        payrollDetail.TotalPayroll = Convert.ToInt32(payrollDetail.FeePayroll + payrollDetail.ResultPayroll);
                        payrollDetail.TaxPayroll = Convert.ToInt32((payrollDetail.FeePayroll * payrollHistory.PpnPercentage) / 100);
                        payrollDetail.GrossPayroll = Convert.ToInt32(payrollDetail.TotalPayroll + payrollDetail.TaxPayroll);
                        payrollDetail.AttributePayroll = Convert.ToInt32(payrollDetail.AtributeBilling);
                        payrollDetail.BpjsTkDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsTk1Percentage) / 100);
                        if (payrollDetail.Employee.BpjsRemark != null)
                        {
                            if (payrollDetail.Employee.BpjsRemark.ToLower().Replace(" ", string.Empty) == "bumbk")
                            {
                                payrollDetail.BpjsKesehatanDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.BpjsPayrollPercentage) / 100);
                            }
                            else
                            {
                                payrollDetail.BpjsKesehatanDeduction = 0;
                            }
                        }
                        else
                        {
                            payrollDetail.BpjsKesehatanDeduction = 0;
                        }

                        payrollDetail.BpjsReturn = payrollDetail.BpjsKesehatanDeduction;
                        payrollDetail.PensionDeduction = Convert.ToInt32((payrollDetail.Employee.Location.UMK * payrollHistory.PensionPayrollPercentage) / 100);
                        payrollDetail.PTKP = Convert.ToInt32(payrollDetail.Employee.FamilyStatus.PTKP);
                        payrollDetail.PKP1 = Convert.ToInt32(payrollDetail.ResultPayroll - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.AnotherDeduction);
                        payrollDetail.PKP2 = Convert.ToInt32(payrollDetail.PKP1 - payrollDetail.PTKP);
                        if (payrollDetail.PKP2 > 1)
                        {
                            payrollDetail.PPH21 = Convert.ToInt32((payrollDetail.PKP2 * payrollDetail.PayrollHistory.Pph21Percentage) / 100);
                        }
                        else
                        {
                            payrollDetail.PPH21 = 0;

                        }

                        payrollDetail.PPH23 = Convert.ToInt32((payrollDetail.FeePayroll * payrollDetail.PayrollHistory.Pph23Percentage) / 100);
                        payrollDetail.Netto = Convert.ToInt32(payrollDetail.ResultPayroll + payrollDetail.Rapel + payrollDetail.BpjsReturn - payrollDetail.BpjsKesehatanDeduction - payrollDetail.PensionDeduction - payrollDetail.BpjsTkDeduction - payrollDetail.PPH21);
                        payrollDetail.TakeHomePay = Convert.ToInt32(payrollDetail.Netto - payrollDetail.AnotherDeduction - payrollDetail.TransferFee);
                        payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                        updatedPayrollDetails.Add(payrollDetail);
                    }
                }

                payrollDB.PayrollDetail.UpdateRange(updatedPayrollDetails);
                await payrollDB.SaveChangesAsync();
                return Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Calculate Assa");
                throw error;
            }
 
        }

        [HttpPost]
        [Route("api/payrollDetail/update/{id}")]
        [Authorize(Roles = "Admin")]
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
                List<PayrollDetail> updatedPayrollDetails = new List<PayrollDetail>();
                //Access File
                bool isFileOk = true;
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

                        if (payrollHistory.MainCustomerId == 1)
                        {

                            foreach (ExcelWorksheet excelWorksheet in excelPackage.Workbook.Worksheets.ToList())
                            {
                                bool isSheetOk = true;
                                AddressPayroll address = new AddressPayroll(excelWorksheet);
                                if (!address.IsValid)
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"G1"].Value = "Format tidak valid";
                                    excelWorksheet.Cells[$"G1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"G1"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                int rowNum = 1;
                                for (int currentRow = address.DataStartRow; currentRow < address.DataEndRow; currentRow++)
                                {
                                    string employeeNIK = GetStringValue(excelWorksheet, address.NIK, currentRow);
                                    List<string> employeeNames = GetStringValue(excelWorksheet, address.Name, currentRow).Split(";").ToList();
                                    PayrollDetail payrollDetail = payrollDetails
                                        .Where(column => column.EmployeeId == employeeNIK)
                                        //                                    .Where(column => employeeNames.Contains(column.Employee.Name))
                                        .FirstOrDefault();
                                    if (payrollDetail == null)
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"A{currentRow}"].Value = "Pekerja tidak terdaftar";
                                        excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        excelWorksheet.Cells[$"{address.Name}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.Name}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;
                                    }

                                    payrollDetail.MainSalaryBilling = GetIntValue(excelWorksheet, address.MainSalaryBilling, currentRow);
                                    payrollDetail.JamsostekBilling = GetIntValue(excelWorksheet, address.JamsostekBilling, currentRow);
                                    payrollDetail.BpjsBilling = GetIntValue(excelWorksheet, address.BpjsBilling, currentRow);
                                    payrollDetail.PensionBilling = GetIntValue(excelWorksheet, address.PensionBilling, currentRow);
                                    payrollDetail.AtributeBilling = GetIntValue(excelWorksheet, address.AtributeBilling, currentRow);
                                    payrollDetail.MainPrice = GetIntValue(excelWorksheet, address.MainPrice, currentRow);
                                    payrollDetail.ManagementFeeBilling = GetIntValue(excelWorksheet, address.ManagementFeeBilling, currentRow);
                                    payrollDetail.InsentiveBilling = address.IsAnyInsentiveBilling ? GetIntValue(excelWorksheet, address.InsentiveBilling, currentRow) : 0;
                                    payrollDetail.AttendanceBilling = address.IsAnyAttendanceBilling ? GetIntValue(excelWorksheet, address.AttendanceBilling, currentRow) : 0;
                                    payrollDetail.AbsentDeduction = address.IsAnyAbsentDeduction ? GetIntValue(excelWorksheet, address.AbsentDeduction, currentRow) : 0;
                                    payrollDetail.AppreciationBilling = address.IsAnyAppreciationBilling ? GetIntValue(excelWorksheet, address.AppreciationBilling, currentRow) : 0;
                                    payrollDetail.OvertimeBilling = address.IsAnyOvertimeBilling ? GetIntValue(excelWorksheet, address.OvertimeBilling, currentRow) : 0;
                                    payrollDetail.AnotherDeduction = address.IsAnyAnotherDeduction ? GetIntValue(excelWorksheet, address.AnotherDeduction, currentRow) : 0;
                                    payrollDetail.PulseAllowance = address.IsAnyPulseAllowance ? GetIntValue(excelWorksheet, address.PulseAllowance, currentRow) : 0;
                                    payrollDetail.SubtotalBilling = GetIntValue(excelWorksheet, address.SubtotalBilling, currentRow);
                                    payrollDetail.TaxBilling = GetIntValue(excelWorksheet, address.TaxBilling, currentRow);
                                    payrollDetail.GrandTotalBilling = GetIntValue(excelWorksheet, address.GrandTotalBilling, currentRow);
                                    if (!payrollDetail.IsValidGrandTotalBilling)
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"A{currentRow}"].Value = "Perhitungan Keliru, silahkan periksa kembali";
                                        excelWorksheet.Cells[$"{address.GrandTotalBilling}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.GrandTotalBilling}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;
                                    }
                                    payrollDetail.PayrollDetailStatusId = 2;
                                    payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                                    updatedPayrollDetails.Add(payrollDetail);

                                    excelWorksheet.Cells[$"A{currentRow}"].Value = "OK";
                                    rowNum++;
                                }

                                if (isSheetOk)
                                {
                                    excelPackage.Workbook.Worksheets.Delete(excelWorksheet);
                                }
                            }

                        }
                        payrollDB.PayrollDetail.UpdateRange(updatedPayrollDetails);
                        await payrollDB.SaveChangesAsync();
                        await CalculateAssa(payrollDetails, payrollHistory);

                        if (!isFileOk)
                        {

                            string excelFileDirectory = $"wwwroot/file/ErrorUpload.xlsx";
                            if (System.IO.File.Exists(excelFileDirectory))
                            {
                                System.IO.File.Delete(excelFileDirectory);
                            }
                            FileInfo excelFile = new FileInfo(excelFileDirectory);
                            await excelPackage.SaveAsAsync(excelFile);
                            return BadRequest($"0");
                        }

                    }

                }
                return Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Update {id}");
                return BadRequest(error.Message);
            }
        }


        [Authorize(Roles = "Admin")]
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
                return BadRequest(error.Message);
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
                return BadRequest(error.Message);
            }
        }

        private string GetStringValue(ExcelWorksheet excelWorksheet, string column, int row)
        {
            string stringResult = null;
            try
            {
                if (excelWorksheet.Cells[$"{column}{row}"].Value != null)
                {
                    stringResult = excelWorksheet.Cells[$"{column}{row}"].Value.ToString().ToLower().Replace(" ", string.Empty);
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Get String Value {row}");
            }
            return stringResult;
        }

        private decimal GetDecimalValue(ExcelWorksheet excelWorksheet, string column, int row)
        {
            decimal decimalResult = 0;
            try
            {
                if (excelWorksheet.Cells[$"{column}{row}"].Value != null)
                {
                    decimalResult = decimal.Parse(excelWorksheet.Cells[$"{column}{row}"].Value.ToString());
                }
                else
                {
                    decimalResult = 0;
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Get decimal Value {row}");
            }
            return decimalResult;
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
