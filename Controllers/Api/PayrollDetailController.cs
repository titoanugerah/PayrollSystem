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

        public PayrollDetailController(ILogger<PayrollDetailController> _logger, PayrollDB _payrollDB)
        {
            payrollDB = _payrollDB;
            logger = _logger;
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
                    .Include(table => table.Employee)
                    .Include(table => table.PayrollHistory)
                    .Where(column => column.PayrollHistoryId == id)
                    .Where(column => column.Employee.Name.Contains(request.Keyword))
                    .Skip(request.Skip)
                    .OrderBy(column => column.Employee.Name)
                    .Take(request.PageSize)
                    .ToListAsync();
                payrollDetailView.RecordsFiltered = await payrollDB.PayrollDetail.CountAsync();
                return new JsonResult(payrollDetailView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Read Datatable");
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
                    .Include(table => table.Employee)
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

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/update/{id}")]
        public async Task<IActionResult> Update(int id, IFormFile file)
        {
            int startRow = 0;
            int endRow = 0;
            try
            {
                List<PayrollDetail> payrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.PayrollHistory)
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
                            startRow = 0;
                            endRow = 0;
                            for (int currentRow = worksheet.Dimension.End.Row; currentRow > 0 ; currentRow--)
                            {
                                if (GetStringValue(worksheet, "A", currentRow) == "total")
                                {
                                    endRow = currentRow;
                                }
                                if (GetStringValue(worksheet, "A", currentRow) == "no")
                                {
                                    startRow = currentRow+1;
                                }
                            }

                            for (int currentRow = startRow; currentRow < endRow; currentRow++)
                            {
                                int employeeId = GetIntValue(worksheet, "B", currentRow);
                                bool isExist = payrollDetails
                                    .Where(column => column.EmployeeId == employeeId)
                                    .Any();
                                if (isExist)
                                {
                                    Employee employee = employees
                                        .Where(column => column.NIK == employeeId)
                                        .FirstOrDefault();
                                    bool isValidEmployee = employee.Name.ToLower().Replace(" ", string.Empty) == GetStringValue(worksheet, "C", currentRow);
                                    bool isExistEmployee = employee.IsExist == true;
                                    if (isValidEmployee & isExistEmployee)
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
                                        payrollDetail.OvertimeBilling = GetIntValue(worksheet, "P", currentRow); 
                                        payrollDetail.SubtotalBilling = GetIntValue(worksheet, "Q", currentRow); 
                                        payrollDetail.TaxBilling = GetIntValue(worksheet, "R", currentRow); 
                                        payrollDetail.GrandTotalBilling = GetIntValue(worksheet, "S", currentRow);
                                        payrollDetail.PayrollDetailStatusId = 2;
                                        payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    payrollDB.PayrollDetail.UpdateRange(payrollDetails);
                                    await payrollDB.SaveChangesAsync();
                                }

                                
                            }

                        }
                    }
                }
                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Update {id}");
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
                    //intResult = Convert.ToInt32(excelWorksheet.Cells[$"{column}{row}"].Value.ToString());
                }
                else
                {
                    intResult = 0;
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
