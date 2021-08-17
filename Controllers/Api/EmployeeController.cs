using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly PayrollDB payrollDB;
        private readonly IHostingEnvironment hostingEnvironment;

        public EmployeeController(ILogger<EmployeeController> _logger, PayrollDB _payrollDB, IHostingEnvironment _hostingEnvironment)
        {
            logger = _logger;
            payrollDB = _payrollDB;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        [Route("api/employee/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(IFormFile file)
        {
            try
            {
                //Check if File valid
                string extension = Path.GetExtension(file.FileName);
                if (extension != ".xlsx" && extension != ".xls")
                {
                    return BadRequest($"format file {extension} tidak diijinkan, silahkan upload file dengan format excel");
                }

                //List Up Needed Data
                MasterData masterData = new MasterData();
                masterData.Banks = await payrollDB.Bank.AsNoTracking().ToListAsync();
                masterData.Customers = await payrollDB.Customer.AsNoTracking().ToListAsync();
                masterData.Districts = await payrollDB.District.AsNoTracking().ToListAsync();
                masterData.Employees = await payrollDB.Employee.AsNoTracking().ToListAsync();
                masterData.EmploymentStatuses = await payrollDB.EmploymentStatus.AsNoTracking().ToListAsync();
                masterData.FamilyStatuses = await payrollDB.FamilyStatus.AsNoTracking().ToListAsync();
                masterData.Locations = await payrollDB.Location.AsNoTracking().ToListAsync();
                masterData.Positions = await payrollDB.Position.AsNoTracking().ToListAsync();
                masterData.Roles = await payrollDB.Role.AsNoTracking().ToListAsync();

                //Declare New & Old Employee
                List<Employee> newEmployees = new List<Employee>();
                List<Employee> oldEmployees = new List<Employee>();

                //Access File
                bool isFileOk = true;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Copy to Memory Stream
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
                    {
                        if (excelPackage.Workbook.Worksheets.Count == 0)
                        {
                            return BadRequest($"Tidak ada worksheet yang tersedia pada file {file.FileName}");
                        }

                        foreach (ExcelWorksheet excelWorksheet in excelPackage.Workbook.Worksheets.ToList())
                        {
                            bool isSheetOk = true;
                            AddressEmployee address = new AddressEmployee(excelWorksheet);
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
                                bool isOldEmployee = false;
                                Employee employee = new Employee();
                                int employeeNIK = GetIntValue(excelWorksheet, address.NIK, currentRow);
                                if (employeeNIK != null)
                                {
                                    isOldEmployee = masterData.Employees.Where(column => column.NIK == employeeNIK).Any();
                                    if (isOldEmployee)
                                    {
                                        employee = masterData.Employees
                                            .Where(column => column.NIK == employeeNIK)
                                            .FirstOrDefault();
                                    }
                                    else
                                    {
                                        employee.NIK = employeeNIK;
                                    }
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "NIK tidak ada";
                                    excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                //NIK
                                if (employeeNIK != null)
                                {
                                    if (masterData.Employees.Where(column => column.NIK == employeeNIK).Any())
                                    {
                                        employee = masterData.Employees
                                            .Where(column => column.NIK == employeeNIK)
                                            .FirstOrDefault();
                                    }
                                    else
                                    {
                                        employee.NIK = employeeNIK;
                                    }
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "NIK kosong";
                                    excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                //IsExist
                                employee.IsExist = GetBoolValue(excelWorksheet, $"{address.IsExist}{currentRow}", "aktif");

                                //Name
                                if (GetStringValue(excelWorksheet, address.Name, currentRow) != null)
                                {
                                    employee.Name = GetStringValue(excelWorksheet, address.Name, currentRow);
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Nama kosong";
                                    excelWorksheet.Cells[$"{address.Name}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.Name}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                if (GetStringValue(excelWorksheet, address.PhoneNumber, currentRow) != null)
                                {
                                    employee.PhoneNumber = GetStringValue(excelWorksheet, address.PhoneNumber, currentRow);
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Nomor HP kosong";
                                    excelWorksheet.Cells[$"{address.PhoneNumber}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.PhoneNumber}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                if (address.IsPositionMultiColumn)
                                {
                                    if (GetStringValue(excelWorksheet, address.DriverPosition, currentRow) != null)
                                    {
                                        bool isAny = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.DriverPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.DriverPosition, currentRow)))
                                            .FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan tidak cocok";
                                            excelWorksheet.Cells[$"{address.DriverPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            excelWorksheet.Cells[$"{address.DriverPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            continue;
                                        }
                                    }
                                    else if (GetStringValue(excelWorksheet, address.HelperPosition, currentRow) != null)
                                    {
                                        bool isAny = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.HelperPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.HelperPosition, currentRow)))
                                            .FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan tidak cocok";
                                            excelWorksheet.Cells[$"{address.HelperPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            excelWorksheet.Cells[$"{address.HelperPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            continue;
                                        }
                                    }
                                    else if (GetStringValue(excelWorksheet, address.CheckerPosition, currentRow) != null)
                                    {
                                        bool isAny = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.CheckerPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.CheckerPosition, currentRow)))
                                            .FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan tidak cocok";
                                            excelWorksheet.Cells[$"{address.CheckerPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            excelWorksheet.Cells[$"{address.CheckerPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            continue;
                                        }
                                    }
                                    else if (GetStringValue(excelWorksheet, address.NonDriverPosition, currentRow) != null)
                                    {
                                        bool isAny = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.NonDriverPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.NonDriverPosition, currentRow)))
                                            .FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan tidak cocok";
                                            excelWorksheet.Cells[$"{address.NonDriverPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            excelWorksheet.Cells[$"{address.NonDriverPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan tidak diisi";
                                        excelWorksheet.Cells[$"{address.DriverPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.DriverPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;

                                    }
                                }
                                else
                                {
                                    if (GetStringValue(excelWorksheet, address.PositionId, currentRow) != null)
                                    {
                                        bool isAny = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.PositionId, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.PositionId, currentRow)))
                                            .FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan tidak cocok";
                                            excelWorksheet.Cells[$"{address.PositionId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            excelWorksheet.Cells[$"{address.PositionId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Jabatan kosong";
                                        excelWorksheet.Cells[$"{address.PositionId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.PositionId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;
                                    }
                                }

                                if (GetStringValue(excelWorksheet, address.LocationId, currentRow) != null)
                                {
                                    bool isAny = masterData.Locations
                                        .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.LocationId, currentRow)))
                                        .Any();
                                    if (isAny)
                                    {
                                        employee.LocationId = masterData.Locations
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.LocationId, currentRow)))
                                            .FirstOrDefault().Id;

                                    }
                                    else
                                    {
                                        employee.LocationId = masterData.Locations
                                            .Where(column => standarize(column.Name) == standarize("bekasi"))
                                            .FirstOrDefault().Id;
                                    }
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Lokasi/Rute kosong";
                                    excelWorksheet.Cells[$"{address.LocationId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.LocationId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                //Customer
                                if (address.CustomerId != null)
                                {

                                    if (GetStringValue(excelWorksheet, address.CustomerId, currentRow) != null)
                                    {
                                        bool isAny = masterData.Customers
                                            .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.CustomerId, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.CustomerId = masterData.Customers
                                                .Where(column => standarize(column.Name) == standarize(GetStringValue(excelWorksheet, address.CustomerId, currentRow)))
                                                .FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Customer tidak sesuai";
                                            excelWorksheet.Cells[$"{address.CustomerId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            excelWorksheet.Cells[$"{address.CustomerId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Customer kosong";
                                        excelWorksheet.Cells[$"{address.CustomerId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.CustomerId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;
                                    }
                                }
                                else
                                {
                                    employee.CustomerId = masterData.Customers
                                        .Where(column => standarize(column.Name) == standarize("Non Customer"))
                                        .FirstOrDefault().Id;
                                }

                                //Family Status Code
                                if (GetStringValue(excelWorksheet, address.FamilyStatusCode, currentRow) != null)
                                {
                                    bool isAny = masterData.FamilyStatuses
                                        .Where(column => standarize(column.Code) == standarize(GetStringValue(excelWorksheet, address.FamilyStatusCode, currentRow)))
                                        .Any();
                                    if (isAny)
                                    {
                                        employee.FamilyStatusCode = masterData.FamilyStatuses
                                            .Where(column => standarize(column.Code) == standarize(GetStringValue(excelWorksheet, address.FamilyStatusCode, currentRow)))
                                            .FirstOrDefault().Code;
                                    }
                                    else
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Status Perkawinan tidak cocok";
                                        excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;
                                    }
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Status Perkawinan kosong";
                                    excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                //DriverLicenseType
                                if(address.DriverLicenseType != null)
                                {
                                    employee.DriverLicenseType = GetStringValue(excelWorksheet, address.DriverLicenseType, currentRow);
                                }

                                //DriverLicense
                                if (address.DriverLicense != null)
                                {
                                    employee.DriverLicense = GetStringValue(excelWorksheet, address.DriverLicense, currentRow);
                                }

                                //Bpjs Number
                                if (address.BpjsNumber != null)
                                {
                                    employee.BpjsNumber = GetStringValue(excelWorksheet, address.BpjsNumber, currentRow);
                                }

                                //Bpjs Remark
                                if (address.BpjsRemark != null)
                                {
                                    employee.BpjsRemark = GetStringValue(excelWorksheet, address.BpjsRemark, currentRow);
                                }

                                //Jamsostek Number
                                if (address.JamsostekNumber != null)
                                {
                                    employee.JamsostekNumber = GetStringValue(excelWorksheet, address.JamsostekNumber, currentRow);
                                }

                                //Jamsostek Remark
                                if (address.JamsostekRemark != null)
                                {
                                    employee.JamsostekRemark = GetStringValue(excelWorksheet, address.JamsostekRemark, currentRow);
                                }

                                //KTP 
                                if (GetStringValue(excelWorksheet, address.KTP, currentRow) != null)
                                {
                                    employee.KTP = GetStringValue(excelWorksheet, address.KTP, currentRow);
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "KTP kosong";
                                    excelWorksheet.Cells[$"{address.KTP}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.KTP}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                //Todo Bank Code
                                if (GetStringValue(excelWorksheet, address.BankCode, currentRow) != null)
                                {
                                    bool isAny = masterData.Banks
                                        .Where(column => standarize(column.Code) == standarize(GetStringValue(excelWorksheet, address.BankCode, currentRow)))
                                        .Any();
                                    if (isAny)
                                    {
                                        employee.BankCode = masterData.Banks
                                            .Where(column => standarize(column.Code) == standarize(GetStringValue(excelWorksheet, address.BankCode, currentRow)))
                                            .FirstOrDefault().Code;
                                    }
                                    else
                                    {
                                        isFileOk = false;
                                        isSheetOk = false;
                                        excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Bank tidak cocok";
                                        excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        continue;
                                    }
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Bank kosong";
                                    excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                // Account Name
                                if (address.AccountName != null)
                                {
                                    employee.AccountName = GetStringValue(excelWorksheet, address.AccountName, currentRow);
                                }

                                //KTP 
                                if (GetStringValue(excelWorksheet, address.AccountNumber, currentRow) != null)
                                {
                                    employee.AccountNumber = GetStringValue(excelWorksheet, address.KTP, currentRow);
                                }
                                else
                                {
                                    isFileOk = false;
                                    isSheetOk = false;
                                    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Nomor Rekening Kosong";
                                    excelWorksheet.Cells[$"{address.AccountNumber}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    excelWorksheet.Cells[$"{address.AccountNumber}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    continue;
                                }

                                if (isOldEmployee)
                                {
                                    payrollDB.Entry(employee).State = EntityState.Modified;
                                    oldEmployees.Add(employee);
                                }
                                else
                                {
                                    payrollDB.Entry(employee).State = EntityState.Added;
                                    newEmployees.Add(employee);
                                }
                            }
                            await payrollDB.Employee.AddRangeAsync(newEmployees);
                            payrollDB.Employee.UpdateRange(oldEmployees);
                            //await payrollDB.SaveChangesAsync();

                            if (isSheetOk)
                            {
                                excelPackage.Workbook.Worksheets.Delete(excelWorksheet);
                            }
                        }

                        if (!isFileOk)
                        {

                            string excelFileDirectory = $"wwwroot/file/ErrorEmployeeUpload.xlsx";
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

                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, "Employee Controller API - Create ");
                return BadRequest(error.Message);
            }
        }

        private string standarize(object keyword)
        {
            string value = null;
            if (keyword != null)
            {
                value = keyword.ToString().ToLower().Replace(" ", string.Empty);
            }
            return value;
        }

        [Authorize]
        [HttpPost]
        [Route("api/employee/create2")]
        public async Task<IActionResult> Create2(IFormFile file)
        {
            try
            {
                MasterData masterData = new MasterData();
                masterData.Banks =  await payrollDB.Bank.AsNoTracking().ToListAsync();
                masterData.Customers = await payrollDB.Customer.AsNoTracking().ToListAsync();
                masterData.Districts = await payrollDB.District.AsNoTracking().ToListAsync();
                masterData.Employees = await payrollDB.Employee.AsNoTracking().ToListAsync();
                masterData.EmploymentStatuses = await payrollDB.EmploymentStatus.AsNoTracking().ToListAsync();
                masterData.FamilyStatuses = await payrollDB.FamilyStatus.AsNoTracking().ToListAsync();
                masterData.Locations = await payrollDB.Location.AsNoTracking().ToListAsync();
                masterData.Positions = await payrollDB.Position.AsNoTracking().ToListAsync();
                masterData.Roles = await payrollDB.Role.AsNoTracking().ToListAsync();
                
                List<Employee> newEmployees = new List<Employee>();
                List<Employee> updateEmployees = new List<Employee>();

                using (MemoryStream stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        bool isError =false;
                        List<WorksheetResult> worksheetResults = new List<WorksheetResult>();
                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        {
                            WorksheetResult worksheetResult = new WorksheetResult();
                            worksheetResult = ReadExcel(worksheet, masterData);
                            if (worksheetResult.Worksheet!=null)
                            {
                                if (worksheetResult.IsError && worksheetResult.IsAcceptableFormat)
                                {
                                    isError = true;
                                    string sheetName = $"{worksheetResult.Worksheet.Name}_REV";
                                    excelPackage.Workbook.Worksheets.Add(sheetName, worksheetResult.Worksheet);

                                }

                            }
                                //excelPackage.Workbook.Worksheets.Delete(worksheet);
                        }

                        if (isError)
                        {
                            string excelFileDirectory = $"wwwroot/file/blank.xlsx";
                            FileInfo excelFile = new FileInfo(excelFileDirectory);
                            await excelPackage.SaveAsAsync(excelFile);
                            return BadRequest(excelFile.FullName.ToString());
                        }

                    }
                }
                return new JsonResult(Ok());

            }
            catch (Exception error)
            {
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
                logger.LogError(error, $"Employee API - Get String Value {row}");
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
                logger.LogError(error, $"Employee API - Get String Value {row}");
                throw error;
            }
        }


        private WorksheetResult ReadExcel(ExcelWorksheet worksheet, MasterData masterData)
        {
            WorksheetResult worksheetResult = new WorksheetResult();
            
            worksheetResult.LastRow = 0;
            try
            {
                MappingExcelEmployee mapping = new MappingExcelEmployee(worksheet);
                List<Employee> newEmployee = new List<Employee>();
                List<Employee> oldEmployee = new List<Employee>();
                
                if (worksheetResult.IsAcceptableFormat = mapping.IsAcceptable)
                {

                    for (int currentRow = mapping.InRowStart; currentRow <= mapping.InRowEnd; currentRow++)
                    {
                        worksheetResult.LastRow = currentRow;
                        Employee employee = new Employee();
                        bool isAny = false;
                        //IS EXIST
                        ExcelBoolReader isExist = new ExcelBoolReader(worksheet, $"{mapping.IsExist}{currentRow}", "AKTIF");
                        if (isExist.IsExist)
                        {
                            employee.IsExist = isExist.Value;
                        }
                        
                        //NIK
                        ExcelIntreader nik = new ExcelIntreader(worksheet, $"{mapping.NIK}{currentRow}");
                        if (nik.IsExist && nik.IsInteger)
                        {
                            isAny = masterData.Employees.Where(column => column.NIK == nik.Value).Any();
                            if (isAny)
                            {
                                employee = masterData.Employees.Where(column => column.NIK == nik.Value).FirstOrDefault();                                
                            }
                            else
                            {
                                employee.NIK = nik.Value;
                            }
                        }
                        else
                        {
                            worksheet.Cells[$"{mapping.NIK}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[$"{mapping.NIK}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            continue;
                        }

                        //NAME
                        ExcelStringReader name = new ExcelStringReader(worksheet, $"{mapping.Name}{currentRow}");
                        if (name.IsExist)
                        {
                            employee.Name = name.Value;
                        }
                        else
                        {
                            worksheet.Cells[$"{mapping.Name}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[$"{mapping.Name}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            continue;
                        }

                        //PHONE NUMBER
                        ExcelStringReader phoneNumber = new ExcelStringReader(worksheet, $"{mapping.PhoneNumber}{currentRow}");
                        if (phoneNumber.IsExist)
                        {
                            employee.PhoneNumber = phoneNumber.Value;
                        }

                        //POSITION
                        ExcelStringReader isDriverPosition = new ExcelStringReader(worksheet, $"{mapping.IsDriverPosition}{currentRow}");
                        if (isDriverPosition.IsExist)
                        {
                            bool isAnyPositionId = masterData.Positions
                                .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isDriverPosition.ValueMerged)
                                .Any();
                            if (isAnyPositionId)
                            {
                                employee.PositionId = masterData.Positions
                                    .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isDriverPosition.ValueMerged)
                                    .FirstOrDefault()
                                    .Id;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.IsDriverPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.IsDriverPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //continue;
                            }
                        }

                        ExcelStringReader isHelperPosition = new ExcelStringReader(worksheet, $"{mapping.IsHelperPosition}{currentRow}");
                        if (isHelperPosition.IsExist)
                        {
                            bool isAnyPositionId = masterData.Positions
                                .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isHelperPosition.ValueMerged)
                                .Any();
                            if (isAnyPositionId)
                            {
                                employee.PositionId = masterData.Positions
                                    .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isHelperPosition.ValueMerged)
                                    .FirstOrDefault()
                                    .Id;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.IsHelperPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.IsHelperPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //                                continue;
                            }
                        }

                        ExcelStringReader isCheckerPosition = new ExcelStringReader(worksheet, $"{mapping.IsCheckerPosition}{currentRow}");
                        if (isCheckerPosition.IsExist)
                        {
                            bool isAnyPosition = masterData.Positions
                                .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isCheckerPosition.ValueMerged)
                                .Any();
                            if (isAnyPosition)
                            {
                                employee.PositionId = masterData.Positions
                                    .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isCheckerPosition.ValueMerged)
                                    .FirstOrDefault()
                                    .Id;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.IsCheckerPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.IsCheckerPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //                                continue;
                            }
                        }

                        ExcelStringReader isNonDriverPosition = new ExcelStringReader(worksheet, $"{mapping.IsNonDriverPosition}{currentRow}");
                        if (isNonDriverPosition.IsExist)
                        {
                            bool isAnyPosition = masterData.Positions
                                .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isNonDriverPosition.ValueMerged)
                                .Any();
                            if (isAnyPosition)
                            {
                                employee.PositionId = masterData.Positions
                                    .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == isNonDriverPosition.ValueMerged)
                                    .FirstOrDefault()
                                    .Id;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.IsNonDriverPosition}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.IsNonDriverPosition}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //                                continue;
                            }
                        }

                        //LOCATION
                        ExcelStringReader locationId = new ExcelStringReader(worksheet, $"{mapping.LocationId}{currentRow}");
                        if (locationId.IsExist)
                        {
                            bool isAnyLocationId = masterData.Locations
                                .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == locationId.ValueMerged)
                                .Any();
                            if (isAnyLocationId)
                            {
                                employee.LocationId = masterData.Locations
                                    .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == locationId.ValueMerged)
                                    .FirstOrDefault()
                                    .Id;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.LocationId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.LocationId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //continue;
                            }
                        }

                        //CUSTOMER
                        ExcelStringReader customerId = new ExcelStringReader(worksheet, $"{mapping.CustomerId}{currentRow}");
                        if (customerId.IsExist)
                        {
                            bool isAnyCustomerId = masterData.Customers
                                .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == customerId.ValueMerged)
                                .Any();
                            if (isAnyCustomerId)
                            {
                                employee.CustomerId = masterData.Customers
                                    .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == customerId.ValueMerged)
                                    .FirstOrDefault()
                                    .Id;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.CustomerId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.CustomerId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //continue;

                            }

                        }

                        //TODO : JoinCustomerDate

                        //ADDRESS
                        //ExcelStringReader address = new ExcelStringReader(worksheet, $"{mapping.Address}{currentRow}");
                        //if (address.IsExist)
                        //{
                        //    employee.Address = address.Value;
                        //}

                        //FAMILY STATUS
                        ExcelStringReader familyStatusCode = new ExcelStringReader(worksheet, $"{mapping.FamilyStatusCode}{currentRow}");
                        if (familyStatusCode.IsExist)
                        {
                            bool isAnyFamilyStatusCode = masterData.FamilyStatuses
                                .Where(column => column.Code.ToLower() == familyStatusCode.ValueMerged)
                                .Any();

                            if (isAnyFamilyStatusCode)
                            {
                                employee.FamilyStatusCode = masterData.FamilyStatuses
                                    .Where(column => column.Code.ToLower() == familyStatusCode.ValueMerged)
                                    .FirstOrDefault()
                                    .Code;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.FamilyStatusCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.FamilyStatusCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //continue;
                            }
                        }

                        ////RELIGION
                        //ExcelStringReader religion = new ExcelStringReader(worksheet, $"{mapping.Religion}{currentRow}");
                        //if (religion.IsExist)
                        //{
                        //    employee.Religion = religion.Value;
                        //}

                        ////SEX
                        //ExcelStringReader sex = new ExcelStringReader(worksheet, $"{mapping.Sex}{currentRow}");
                        //if (sex.IsExist)
                        //{
                        //    employee.Sex = sex.Value;
                        //}

                        ////BIRTH PLACE
                        //ExcelStringReader birthPlace = new ExcelStringReader(worksheet, $"{mapping.BirthPlace}{currentRow}");
                        //if (birthPlace.IsExist)
                        //{
                        //    employee.BirthPlace = birthPlace.Value;
                        //}

                        //TODO : BirthDate
                        //TODO : StartContract
                        //TODO : EndContract

                        //EmployeeStatusId
                        //ExcelStringReader employmentStatusId = new ExcelStringReader(worksheet, $"{mapping.EmploymentStatusId}{currentRow}");
                        //if (employmentStatusId.IsExist)
                        //{
                        //    bool isAnyEmploymentStatusId = masterData.EmploymentStatuses
                        //        .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == employmentStatusId.ValueMerged)
                        //        .Any();

                        //    if (isAnyEmploymentStatusId)
                        //    {
                        //        employee.EmploymentStatusId = masterData.EmploymentStatuses
                        //            .Where(column => column.Name.ToLower().Replace(" ", string.Empty) == employmentStatusId.ValueMerged)
                        //            .FirstOrDefault()
                        //            .Id;
                        //    }
                        //    else
                        //    {
                        //        worksheet.Cells[$"{mapping.EmploymentStatusId}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //        worksheet.Cells[$"{mapping.EmploymentStatusId}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        //        //continue;
                        //    }
                        //}

                        //DriverLicenseType
                        ExcelStringReader driverLicenseType = new ExcelStringReader(worksheet, $"{mapping.DriverLicenseType}{currentRow}");
                        if (driverLicenseType.IsExist)
                        {
                            employee.DriverLicenseType = driverLicenseType.Value;
                        }

                        //DriverLicense
                        ExcelStringReader driverLicense = new ExcelStringReader(worksheet, $"{mapping.DriverLicense}{currentRow}");
                        if (driverLicense.IsExist)
                        {
                            employee.DriverLicense = driverLicense.Value;
                        }

                        //Driver License Expire

                        //Has Uniform
                        //ExcelBoolReader hasUniform = new ExcelBoolReader(worksheet, $"{mapping.HasUniform}{currentRow}", "sudah", "nondriver");
                        //if (hasUniform.IsExist)
                        //{
                        //    employee.HasUniform = hasUniform.Value;
                        //}

                        ////TODO : DeliveryUniform

                        ////Has ID Card
                        //ExcelBoolReader hasIdCard = new ExcelBoolReader(worksheet, $"{mapping.HasIdCard}{currentRow}", "sudah", "nondriver");
                        //if (hasIdCard.IsExist)
                        //{
                        //    employee.HasIdCard = hasIdCard.Value;
                        //}

                        ////TODO : DeliveryIdCard

                        ////HAS TRAINING
                        //ExcelBoolReader hasTraining = new ExcelBoolReader(worksheet, $"{mapping.HasTraining}{currentRow}", "sudah", "nondriver");
                        //if (hasTraining.IsExist)
                        //{
                        //    employee.HasTraining = hasTraining.Value;
                        //}

                        ////TRAINING REMARK
                        //ExcelStringReader trainingRemark = new ExcelStringReader(worksheet, $"{mapping.TrainingRemark}{currentRow}");
                        //if (trainingRemark.IsExist)
                        //{
                        //    employee.TrainingRemark = trainingRemark.Value;
                        //}

                        ////TRAINING GRADE
                        //ExcelStringReader trainingGrade = new ExcelStringReader(worksheet, $"{mapping.TrainingGrade}{currentRow}");
                        //if (trainingGrade.IsExist)
                        //{
                        //    employee.TrainingGrade = trainingGrade.Value;
                        //}

                        //BPJS KESEHATAN
                        ExcelStringReader bpjsNumber = new ExcelStringReader(worksheet, $"{mapping.BpjsNumber}{currentRow}");
                        if (bpjsNumber.IsExist)
                        {
                            employee.BpjsNumber = bpjsNumber.Value;
                        }

                        //BPJS REMARK
                        ExcelStringReader bpjsRemark = new ExcelStringReader(worksheet, $"{mapping.BpjsRemark}{currentRow}");
                        if (bpjsRemark.IsExist)
                        {
                            employee.BpjsRemark = bpjsRemark.Value;
                        }

                        //JAMSOSTEK NUMBER
                        ExcelStringReader jamsostekNumber = new ExcelStringReader(worksheet, $"{mapping.JamsostekNumber}{currentRow}");
                        if (jamsostekNumber.IsExist)
                        {
                            employee.JamsostekNumber = jamsostekNumber.Value;
                        }

                        //JAMSOSTEK REMARK
                        ExcelStringReader jamsostekRemark = new ExcelStringReader(worksheet, $"{mapping.JamsostekRemark}{currentRow}");
                        if (jamsostekRemark.IsExist)
                        {
                            employee.JamsostekRemark = jamsostekRemark.Value;
                        }

                        //NPWP
                        ExcelStringReader npwp = new ExcelStringReader(worksheet, $"{mapping.NPWP}{currentRow}");
                        if (npwp.IsExist)
                        {
                            employee.NPWP = npwp.Value;
                        }

                        //KTP
                        ExcelStringReader ktp = new ExcelStringReader(worksheet, $"{mapping.KTP}{currentRow}");
                        if (ktp.IsExist)
                        {
                            employee.KTP = ktp.Value;
                        }

                        //ACCOUNT NAME
                        ExcelStringReader accountName = new ExcelStringReader(worksheet, $"{mapping.AccountName}{currentRow}");
                        if (accountName.IsExist)
                        {
                            employee.AccountName = accountName.Value;
                        }

                        //BANK CODE
                        ExcelStringReader bankCode = new ExcelStringReader(worksheet, $"{mapping.BankCode}{currentRow}");
                        if (bankCode.IsExist)
                        {
                            bool isAnyBank = masterData.Banks
                                .Where(column => column.Code.ToLower().Replace(" ", string.Empty) == bankCode.ValueMerged)
                                .Any();

                            if (isAnyBank)
                            {
                                employee.BankCode = masterData.Banks
                                    .Where(column => column.Code.ToLower().Replace(" ", string.Empty) == bankCode.ValueMerged)
                                    .FirstOrDefault()
                                    .Code;
                            }
                            else
                            {
                                worksheet.Cells[$"{mapping.BankCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[$"{mapping.BankCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                continue;
                            }
                        }

                        ////KK
                        //ExcelStringReader kk = new ExcelStringReader(worksheet, $"{mapping.KK}{currentRow}");
                        //if (kk.IsExist)
                        //{
                        //    employee.KK = kk.Value;
                        //}

                        if (isAny)
                        {
                            payrollDB.Entry(employee).State = EntityState.Modified;
                            oldEmployee.Add(employee);
                        }
                        else
                        {
                            employee.RoleId = 2;
                            using (MD5 md5Hash = MD5.Create())
                            {
                                employee.Password = GetMd5Hash(md5Hash, employee.NIK.ToString());
                            }
                            payrollDB.Entry(employee).State = EntityState.Added;
                            newEmployee.Add(employee);
                        }
                    }
                    payrollDB.Employee.UpdateRange(oldEmployee);
                    payrollDB.Employee.AddRange(newEmployee);
                    payrollDB.SaveChanges();
                    worksheetResult.IsError = false;
                }
                else
                {
                    worksheetResult.IsError = true;
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, "Employee API - Read Excel");
                worksheetResult.IsError = true;
                worksheetResult.ErrorMessage = $"Error at sheet {worksheet.Name} row {worksheetResult.LastRow} : {error.InnerException}";
                //throw error;
            }
            worksheetResult.Worksheet = worksheet;
            return worksheetResult;
        }


        private bool GetBoolValue(ExcelWorksheet worksheet, string stringCell, string stringIfTrue=null, string stringIfFalse =null)
        {
            bool result =false;
            ExcelRangeBase cell = worksheet.Cells[$"{stringCell}"];
            if (cell.Value != null)
            {
                List<string> valueIfTrue = stringIfTrue != null ? stringIfTrue.ToLower().Replace(" ", string.Empty).Split(";").ToList() : null;
                List<string> valueIfFalse = stringIfFalse != null ?stringIfFalse.ToLower().Replace(" ", string.Empty).Split(";").ToList() : null;

                if (valueIfTrue.Contains(standarize(cell.Value)))
                {
                    result =  true;
                }
                else if (valueIfFalse.Contains(standarize(cell.Value)))
                {
                    result =  false;
                }
            }
            return result;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        [Authorize]
        [HttpPost]
        [Route("api/employee/readDatatable/{id}")]
        public  async Task<IActionResult> ReadDatatable(int id = 0)
        {
            try
            {

                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                EmployeeView employeeView = new EmployeeView();
                if (id == 0)
                {
                    employeeView.Data = await payrollDB.Employee
                        .Include(table => table.Location.District)
                        .Include(table => table.Position)
                        .Include(table => table.Customer)
                        .Where(column => column.Name.Contains(request.Keyword) || column.Location.Name.Contains(request.Keyword) || column.Customer.Name.Contains(request.Keyword) || column.Position.Name.Contains(request.Keyword) || column.NIK.ToString().Contains(request.Keyword))
                        .OrderBy(column => column.Location.DistrictId)
                        .Skip(request.Skip)
                        .Take(request.PageSize)
                        .ToListAsync();
                    employeeView.RecordsFiltered = await payrollDB.Employee
                        .Where(column => column.IsExist == true)
                        .CountAsync();
                }
                else
                {
                    employeeView.Data = await payrollDB.Employee
                       .Include(table => table.Location.District)
                       .Include(table => table.Position)
                       .Include(table => table.Customer)
                       .Where(column => column.Location.District.Id == id )
                       .Where(column => column.Name.Contains(request.Keyword) || column.Location.Name.Contains(request.Keyword) || column.Customer.Name.Contains(request.Keyword) || column.Position.Name.Contains(request.Keyword) || column.NIK.ToString().Contains(request.Keyword))
                       .OrderBy(column => column.Location.DistrictId)
                       .Skip(request.Skip)
                       .Take(request.PageSize)
                       .ToListAsync();
                    employeeView.RecordsFiltered = await payrollDB.Employee
                        .Where(column => column.IsExist == true)
                        .Where(column => column.Location.District.Id == id)
                        .CountAsync();
                }
                return Ok(employeeView);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Employee API Controller - Read");
                throw error;
            }
        }

        [HttpGet]
        [Route("api/employee/readDetail/{nik}")]
        [Authorize]
        public async Task<IActionResult> ReadDetail(int nik)
        {
            try
            {
                Employee employee = await payrollDB.Employee
                    .Where(column => column.NIK == nik)
                    .FirstOrDefaultAsync();
                return new JsonResult(employee);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Employee API Controller - Read");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/employee/update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm]EmployeeInput employeeInput, int id)
        {
            try
            {
                Employee employee = await payrollDB.Employee
                    .Where(column => column.NIK == id)
                    .FirstOrDefaultAsync();
                employee.Name = employeeInput.Name;
                //employee.BirthPlace = employeeInput.BirthPlace;
                //employee.BirthDate = employeeInput.BirthDate;
                //employee.Sex = employeeInput.Sex;
                //employee.Religion = employeeInput.Religion;
                //employee.Address = employeeInput.Address;
                employee.PhoneNumber = employeeInput.PhoneNumber;
                employee.KTP = employeeInput.KTP;
                //employee.KK = employeeInput.KK;
                employee.NPWP = employeeInput.NPWP;
                employee.JamsostekNumber = employeeInput.JamsostekNumber;
                employee.JamsostekRemark = employeeInput.JamsostekRemark;
                employee.BpjsNumber = employeeInput.BpjsNumber;
                employee.BpjsRemark = employeeInput.BpjsRemark;
                employee.DriverLicense = employeeInput.DriverLicense;
                employee.DriverLicenseType = employeeInput.DriverLicenseType;
                //employee.DriverLicenseExpire = employeeInput.DriverLicenseExpire;
                employee.AccountNumber = employeeInput.AccountNumber;
                employee.AccountName = employeeInput.AccountName;
                employee.BankCode = employeeInput.BankCode;
                employee.FamilyStatusCode = employeeInput.FamilyStatusCode;
                //employee.EmploymentStatusId = employeeInput.EmploymentStatusId;
                employee.PositionId = employeeInput.PositionId;
                employee.CustomerId = employeeInput.CustomerId;
                employee.LocationId = employeeInput.LocationId;
                employee.RoleId = employeeInput.RoleId;
                //employee.StartContract = employeeInput.StartContract;
                //employee.EndContract = employeeInput.EndContract;
                //employee.JoinCompanyDate = employeeInput.JoinCompanyDate;
                //employee.JoinCustomerDate = employeeInput.JoinCustomerDate;
                //employee.HasUniform = employeeInput.HasUniform;
                //employee.UniformDeliveryDate = employeeInput.UniformDeliveryDate;
                //employee.HasIdCard = employeeInput.HasIdCard;
                //employee.IdCardDeliveryDate = employeeInput.IdCardDeliveryDate;
                //employee.HasTraining = employeeInput.HasTraining;
                //employee.TrainingDeliveryDate = employeeInput.TrainingDeliveryDate;
                //employee.TrainingName = employeeInput.TrainingName;
                //employee.TrainingRemark = employeeInput.TrainingRemark;
                //employee.TrainingGrade = employeeInput.TrainingGrade;
                employee.IsExist = employeeInput.IsExist;
                payrollDB.Entry(employee).State = EntityState.Modified;
                payrollDB.Employee.Update(employee);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(employee);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Employee API - Update {id}");
                return BadRequest(error.Message);
            }
        }

    }
}
