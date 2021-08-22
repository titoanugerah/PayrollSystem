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
            ExcelWorksheet currentWorksheet = null;
            try
            {
                //Check if File valid
                string extension = Path.GetExtension(file.FileName);
                if (extension != ".xlsx" && extension != ".xls")
                {
                    return BadRequest($"format file {extension} tidak diijinkan, silahkan upload file dengan format excel");
                }
                List<Employee> newEmployees = new List<Employee>();
                List<Employee> oldEmployees = new List<Employee>();
                //List Up Needed Data
                MasterData masterData = new MasterData();
                masterData.Banks = await payrollDB.Bank.AsNoTracking().ToListAsync();
                masterData.Customers = await payrollDB.Customer.AsNoTracking().ToListAsync();
                masterData.Districts = await payrollDB.District.AsNoTracking().ToListAsync();
                masterData.Employees = await payrollDB.Employee.AsNoTracking().ToListAsync();
                masterData.FamilyStatuses = await payrollDB.FamilyStatus.AsNoTracking().ToListAsync();
                masterData.Locations = await payrollDB.Location.AsNoTracking().ToListAsync();
                masterData.Positions = await payrollDB.Position.AsNoTracking().ToListAsync();
                masterData.Roles = await payrollDB.Role.AsNoTracking().ToListAsync();


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
                        //Declare New & Old Employee



                        if (excelPackage.Workbook.Worksheets.Count == 0)
                        {
                            return BadRequest($"Tidak ada worksheet yang tersedia pada file {file.FileName}");
                        }

                        foreach (ExcelWorksheet excelWorksheet in excelPackage.Workbook.Worksheets.ToList())
                        {
                            currentWorksheet = excelWorksheet;
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

                            for (int currentRow = address.DataStartRow; currentRow <= address.DataEndRow; currentRow++)
                            {
                                bool isOldEmployee = false;
                                Employee employee = new Employee();
                                string employeeNIK = GetStringValue(excelWorksheet, address.NIK, currentRow);
                               
                                //NIK
                                if (employeeNIK != null)
                                {
                                    if (oldEmployees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Any())
                                    {
                                        if (oldEmployees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Where(column => column.IsExist == false).Any())
                                        {
                                            oldEmployees.Remove(oldEmployees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Where(column => column.IsExist == false).FirstOrDefault());
                                            employee.NIK = excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Value.ToString(); ;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = $"Data sudah ada";
                                            continue;
                                        }

                                    }
                                    else if (newEmployees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Any())
                                    {
                                        if (newEmployees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Where(column => column.IsExist == false).Any())
                                        {
                                            newEmployees.Remove(newEmployees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Where(column => column.IsExist == false).FirstOrDefault());
                                            employee.NIK = excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Value.ToString(); ;
                                        }
                                        else
                                        {
                                            isFileOk = false;
                                            isSheetOk = false;
                                            excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = $"Data sudah ada";
                                            continue;

                                        }

                                    }
                                    else if (masterData.Employees.Where(column => Standarize(column.NIK) == Standarize(employeeNIK)).Any())
                                    {
                                        employee = masterData.Employees
                                            .Where(column => Standarize(column.NIK) == Standarize(employeeNIK))
                                            .FirstOrDefault();
                                        isOldEmployee = true;
                                    }
                                    else
                                    {
                                        employee.NIK = excelWorksheet.Cells[$"{address.NIK}{currentRow}"].Value.ToString(); ;
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
                                    //employee.Name = GetStringValue(excelWorksheet, address.Name, currentRow);
                                    employee.Name = excelWorksheet.Cells[$"{address.Name}{currentRow}"].Value.ToString();
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
                                    //isFileOk = false;
                                    //isSheetOk = false;
                                    //excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Nomor HP kosong";
                                    //excelWorksheet.Cells[$"{address.PhoneNumber}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //excelWorksheet.Cells[$"{address.PhoneNumber}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    //continue;
                                }

                                if (address.IsPositionMultiColumn)
                                {
                                    if (GetStringValue(excelWorksheet, address.DriverPosition, currentRow) != null)
                                    {
                                        bool isAny = masterData.Positions
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.DriverPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.DriverPosition, currentRow)))
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
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.HelperPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.HelperPosition, currentRow)))
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
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.CheckerPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.CheckerPosition, currentRow)))
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
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.NonDriverPosition, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.NonDriverPosition, currentRow)))
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
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.PositionId, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.PositionId = masterData.Positions
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.PositionId, currentRow)))
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
                                        .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.LocationId, currentRow)))
                                        .Any();
                                    if (isAny)
                                    {
                                        employee.LocationId = masterData.Locations
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.LocationId, currentRow)))
                                            .FirstOrDefault().Id;

                                    }
                                    else
                                    {
                                        employee.LocationId = masterData.Locations
                                            .Where(column => Standarize(column.Name) == Standarize("bekasi"))
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
                                            .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.CustomerId, currentRow)))
                                            .Any();
                                        if (isAny)
                                        {
                                            employee.CustomerId = masterData.Customers
                                                .Where(column => Standarize(column.Name) == Standarize(GetStringValue(excelWorksheet, address.CustomerId, currentRow)))
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
                                        .Where(column => Standarize(column.Name) == Standarize("Non Customer"))
                                        .FirstOrDefault().Id;
                                }

                                //Family Status Code
                                if (GetStringValue(excelWorksheet, address.FamilyStatusCode, currentRow) != null)
                                {
                                    bool isAny = masterData.FamilyStatuses
                                        .Where(column => Standarize(column.Code) == Standarize(GetStringValue(excelWorksheet, address.FamilyStatusCode, currentRow)))
                                        .Any();
                                    if (isAny)
                                    {
                                        employee.FamilyStatusCode = masterData.FamilyStatuses
                                            .Where(column => Standarize(column.Code) == Standarize(GetStringValue(excelWorksheet, address.FamilyStatusCode, currentRow)))
                                            .FirstOrDefault().Code;
                                    }
                                    else
                                    {
                                        employee.FamilyStatusCode = "K";
                                        //isFileOk = false;
                                        //isSheetOk = false;
                                        //excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Status Perkawinan tidak cocok";
                                        //excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        //excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                        //continue;
                                    }
                                }
                                else
                                {
                                    employee.FamilyStatusCode = "K";
                                    //isFileOk = false;
                                    //isSheetOk = false;
                                    //excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Status Perkawinan kosong";
                                    //excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //excelWorksheet.Cells[$"{address.FamilyStatusCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    //continue;
                                }

                                //DriverLicenseType
                                if (address.DriverLicenseType != null)
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
                                //else
                                //{
                                //    isFileOk = false;
                                //    isSheetOk = false;
                                //    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "KTP kosong";
                                //    excelWorksheet.Cells[$"{address.KTP}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                //    excelWorksheet.Cells[$"{address.KTP}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //    continue;
                                //}

                                //Todo Bank Code
                                if (GetStringValue(excelWorksheet, address.BankCode, currentRow) != null)
                                {
                                    bool isAny = masterData.Banks
                                        .Where(column => Standarize(column.Code) == Standarize(GetStringValue(excelWorksheet, address.BankCode, currentRow)))
                                        .Any();
                                    if (isAny)
                                    {
                                        employee.BankCode = masterData.Banks
                                            .Where(column => Standarize(column.Code) == Standarize(GetStringValue(excelWorksheet, address.BankCode, currentRow)))
                                            .FirstOrDefault().Code;
                                    }
                                    //else
                                    //{
                                    //    isFileOk = false;
                                    //    isSheetOk = false;
                                    //    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Bank tidak cocok";
                                    //    excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    //    excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    //    continue;
                                    //}
                                }
                                //else
                                //{
                                //    isFileOk = false;
                                //    isSheetOk = false;
                                //    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Bank kosong";
                                //    excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                //    excelWorksheet.Cells[$"{address.BankCode}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //    continue;
                                //}

                                // Account Name
                                if (address.AccountName != null)
                                {
                                    employee.AccountName = GetStringValue(excelWorksheet, address.AccountName, currentRow);
                                }

                                //Account Number
                                if (GetStringValue(excelWorksheet, address.AccountNumber, currentRow) != null)
                                {
                                    employee.AccountNumber = GetStringValue(excelWorksheet, address.AccountNumber, currentRow);
                                }
                                //else
                                //{
                                //    isFileOk = false;
                                //    isSheetOk = false;
                                //    excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "Nomor Rekening Kosong";
                                //    excelWorksheet.Cells[$"{address.AccountNumber}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                //    excelWorksheet.Cells[$"{address.AccountNumber}{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                //    continue;
                                //}

                                excelWorksheet.Cells[$"{address.No}{currentRow}"].Value = "OK";
                                employee.MainCustomerId = address.MainCustomerId;
                                //Todo Password
                                //employee.Password = GetMd5Hash(new MD5(), "lala");
                                using (MD5 md5Hash = MD5.Create())
                                {
                                    employee.Password = GetMd5Hash(md5Hash, employee.NIK);
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
                                payrollDB.Entry(employee).State = EntityState.Detached;
                            }

                            
                            if (isSheetOk)
                            {
                                excelPackage.Workbook.Worksheets.Delete(excelWorksheet);
                            }
                        }

                            payrollDB.Employee.AddRange(newEmployees);
                            payrollDB.Employee.UpdateRange(oldEmployees);
                            await payrollDB.SaveChangesAsync();
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

                return new JsonResult("");
            }
          catch (Exception error)
            {
                logger.LogError(error, "Employee Controller API - Create ");
                return BadRequest($"{currentWorksheet.Name}{error.Message}");
            }
        }

        private string Standarize(object keyword)
        {
            string value = null;
            if (keyword != null)
            {
                value = keyword.ToString().ToLower().Replace(" ", string.Empty);
            }
            return value;
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

        private bool GetBoolValue(ExcelWorksheet worksheet, string stringCell, string stringIfTrue=null, string stringIfFalse =null)
        {
            bool result =false;
            ExcelRangeBase cell = worksheet.Cells[$"{stringCell}"];
            if (cell.Value != null)
            {
                List<string> valueIfTrue = stringIfTrue != null ? Standarize(stringIfTrue).Split(";").ToList() : null;
                List<string> valueIfFalse = stringIfFalse != null ? Standarize(stringIfFalse).Split(";").ToList() : null;

                if (valueIfTrue.Count() > 0)
                {
                    if (valueIfTrue.Contains(Standarize(cell.Value)))
                    {
                        result =  true;
                    }
                    else
                    {
                        result = false;
                    }

                }
                else if (valueIfFalse.Count() > 0)
                {

                    if (valueIfFalse.Contains(Standarize(cell.Value)))
                    {
                        result =  false;
                    }
                    else
                    {
                        result = true;
                    }
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
                        .Where(column => column.Name.Contains(request.Keyword) || column.Location.Name.Contains(request.Keyword) || column.Customer.Name.Contains(request.Keyword) || column.Position.Name.Contains(request.Keyword) || column.NIK.Contains(request.Keyword))
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
                       .Where(column => column.Location.District.Id == id)
                       .Where(column => column.Name.Contains(request.Keyword) || column.Location.Name.Contains(request.Keyword) || column.Customer.Name.Contains(request.Keyword) || column.Position.Name.Contains(request.Keyword) || column.NIK.Contains(request.Keyword))
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
        public async Task<IActionResult> ReadDetail(string nik)
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
        public async Task<IActionResult> Update([FromForm]EmployeeInput employeeInput, string id)
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
