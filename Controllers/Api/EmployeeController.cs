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

        [Authorize] 
        [HttpPost]
        [Route("api/employee/create")]
        public async Task<IActionResult> Create(IFormFile file)
        {
            int row = 0;
            string value = null;
            try
            {
                List<Bank> banks = await payrollDB.Bank.ToListAsync();
                List<Customer> customers = await payrollDB.Customer.ToListAsync();
                List<District> districts = await payrollDB.District.ToListAsync();
                List<Employee> employees = await payrollDB.Employee.ToListAsync();
                List<EmploymentStatus> employmentStatuses = await payrollDB.EmploymentStatus.ToListAsync();
                List<FamilyStatus> familyStatuses = await payrollDB.FamilyStatus.ToListAsync();
                List<Location> locations = await payrollDB.Location.ToListAsync();
                List<Position> positions = await payrollDB.Position.ToListAsync();
                List<Role> roles = await payrollDB.Role.ToListAsync();
                List<Employee> newEmployees = new List<Employee>();
                List<Employee> updateEmployees = new List<Employee>();

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var excelPackage = new ExcelPackage(stream))
                    {
                        bool isAllValid = true;
                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        {
                            ExcelCellAddress start = worksheet.Dimension.Start;
                            ExcelCellAddress end = worksheet.Dimension.End;
                            for (row = 7; row <= end.Row; row++)
                            {
                                bool isValid = true;
                                bool isExist = false;
                                Employee employee = employees
                                    .Where(column => column.NIK == int.Parse(worksheet.Cells[$"C{row}"].Value.ToString().Replace(" ", string.Empty)))
                                    .FirstOrDefault();

                                if (employee!=null)
                                {
                                    isExist = true;
                                }
                                else
                                {
                                    isExist = false;
                                    employee = new Employee();
                                }                 

                                if (worksheet.Cells[$"A{row}"].Value.ToString().ToLower() == "aktif")
                                {
                                    employee.IsExist = true;
                                }
                                else
                                {
                                    employee.IsExist = false;
                                }

                                if (worksheet.Cells[$"C{row}"].Value != null)
                                {
                                    employee.NIK = int.Parse(worksheet.Cells[$"C{row}"].Value.ToString().Replace(" ", string.Empty));
                                    value = $"NIK : {employee.NIK}";
                                }
                                else
                                {
                                    worksheet.Cells[$"C{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"D{row}"].Value != null)
                                {
                                    employee.Name = worksheet.Cells[$"D{row}"].Value.ToString();
                                    value = $"Name : {employee.Name}";
                                }
                                else
                                {
                                    worksheet.Cells[$"D{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"F{row}"].Value != null)
                                {
                                    employee.PhoneNumber = worksheet.Cells[$"F{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"PhoneNumber : {employee.PhoneNumber}";
                                }

                                if (worksheet.Cells[$"G{row}"].Value != null)
                                {
                                    employee.PositionId = positions.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"G{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Id;
                                    value = $"PositionId : {employee.PositionId}";
                                }
                                else if (worksheet.Cells[$"H{row}"].Value != null)
                                {
                                    employee.PositionId = positions.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"H{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Id;
                                    value = $"PositionId : {employee.PositionId}";
                                }
                                else if (worksheet.Cells[$"I{row}"].Value != null)
                                {
                                    employee.PositionId = positions.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"I{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Id;
                                    value = $"PositionId : {employee.PositionId}";
                                }
                                else if (worksheet.Cells[$"J{row}"].Value != null)
                                {
                                    employee.PositionId = positions.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"J{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Id;
                                    value = $"PositionId : {employee.PositionId}";
                                }
                                else
                                {
                                    worksheet.Cells[$"G{row}"].Value = "Belum Diisi";
                                    worksheet.Cells[$"H{row}"].Value = "Belum Diisi";
                                    worksheet.Cells[$"I{row}"].Value = "Belum Diisi";
                                    worksheet.Cells[$"J{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"K{row}"].Value != null)
                                {
                                    string locationName = worksheet.Cells[$"K{row}"].Value.ToString().Replace(" ",string.Empty);
                                    employee.LocationId = locations.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == locationName.ToLower()).FirstOrDefault().Id;
                                    value = $"LocationId : {employee.LocationId}";
                                }

                                if (worksheet.Cells[$"L{row}"].Value != null)
                                {
                                    employee.CustomerId = customers.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"L{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Id;
                                    value = $"CustomerId: {employee.CustomerId}";
                                }

                                if (worksheet.Cells[$"N{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"N{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.JoinCustomerDate = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.JoinCustomerDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"N{row}"].Value.ToString()));
                                    }
                                    value = $"JoinCustomerDate  : {employee.JoinCustomerDate}";
                                }
                                else
                                {
                                    worksheet.Cells[$"N{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"O{row}"].Value != null)
                                {
                                    employee.Address = worksheet.Cells[$"O{row}"].Value.ToString();
                                    value = $"Address : {employee.Address}";
                                }
                                else
                                {
                                    worksheet.Cells[$"O{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"P{row}"].Value != null)
                                {
                                    employee.FamilyStatusCode = familyStatuses.Where(column => column.Code.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"P{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Code;
                                    value = $"FamilyStatusCode : {employee.FamilyStatusCode}";
                                }
                                else
                                {
                                    worksheet.Cells[$"P{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"Q{row}"].Value != null)
                                {
                                    employee.Religion = worksheet.Cells[$"Q{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"Religion : {employee.Religion}";
                                }
                                else
                                {
                                    worksheet.Cells[$"Q{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"R{row}"].Value != null)
                                {
                                    employee.Sex = worksheet.Cells[$"R{row}"].Value.ToString();
                                    value = $"Sex : {employee.Sex}";
                                }
                                else
                                {
                                    worksheet.Cells[$"R{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"T{row}"].Value != null)
                                {
                                    employee.BirthPlace = worksheet.Cells[$"T{row}"].Value.ToString();
                                    value = $"BirthPlace : {employee.BirthPlace}";
                                }
                                else
                                {
                                    worksheet.Cells[$"T{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"U{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"U{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.BirthDate = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.BirthDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"U{row}"].Value.ToString()));
                                    }
                                    value = $"BirthDate : {employee.BirthDate}";
                                }
                                else
                                {
                                    worksheet.Cells[$"U{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"V{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"V{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.StartContract = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.StartContract = DateTime.FromOADate(int.Parse(worksheet.Cells[$"V{row}"].Value.ToString()));
                                    }
                                    value = $"StartContract : {employee.StartContract}";

                                }
                                else
                                {
                                    worksheet.Cells[$"V{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"W{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"W{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.EndContract = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.EndContract = DateTime.FromOADate(int.Parse(worksheet.Cells[$"W{row}"].Value.ToString()));
                                    }
                                    value = $"EndContract : {employee.EndContract}";
                                }
                                else
                                {
                                    worksheet.Cells[$"W{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"X{row}"].Value != null)
                                {
                                    employee.EmploymentStatusId = employmentStatuses.Where(column => column.Name.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"X{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Id;
                                    value = $"EmploymentStatusId : {employee.EmploymentStatusId}";
                                }
                                else
                                {
                                    worksheet.Cells[$"X{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"AA{row}"].Value != null)
                                {
                                    employee.DriverLicenseType = worksheet.Cells[$"AA{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"DriverLicense : {employee.DriverLicenseType}";
                                }


                                if (worksheet.Cells[$"AB{row}"].Value != null)
                                {
                                    employee.DriverLicense = worksheet.Cells[$"AB{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"DriverLicense : {employee.DriverLicense}";
                                }

                                if (worksheet.Cells[$"AC{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"AC{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.DriverLicenseExpire = dateTimeString;
                                    }
                                    else if(worksheet.Cells[$"AC{row}"].Value.ToString().ToLower().Replace(" ", string.Empty) == "seumurhidup")
                                    {
                                        employee.DriverLicenseExpire = DateTime.MaxValue;
                                    }
                                    else if (worksheet.Cells[$"AC{row}"].Value.ToString().ToLower().Replace(" ", string.Empty) == "nondriver")
                                    {
                                        //employee.DriverLicenseExpire ;
                                    }
                                    else
                                    {
                                        employee.DriverLicenseExpire = DateTime.FromOADate(int.Parse(worksheet.Cells[$"AC{row}"].Value.ToString()));
                                    }
                                    value = $"DriverLicenseExpire : {employee.DriverLicenseExpire}";
                                }

                                if (worksheet.Cells[$"AD{row}"].Value.ToString().ToLower() == "sudah")
                                {
                                    employee.HasUniform = true;
                                    value = $"HasUniform : {employee.HasUniform}";
                                }

                                if (worksheet.Cells[$"AE{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"AE{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.UniformDeliveryDate = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.UniformDeliveryDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"AE{row}"].Value.ToString()));
                                    }
                                    value = $"Uniform Delivery Date : {employee.UniformDeliveryDate}";
                                }

                                if (worksheet.Cells[$"AF{row}"].Value.ToString().ToLower() == "sudah")
                                {
                                    employee.HasIdCard = true;
                                    value = $"HasIDCard : {employee.HasIdCard}";
                                }

                                if (worksheet.Cells[$"AG{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"AG{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.IdCardDeliveryDate = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.IdCardDeliveryDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"AG{row}"].Value.ToString()));
                                    }
                                    value = $"ID Card Delivery Date : {employee.IdCardDeliveryDate}";
                                }

                                if (worksheet.Cells[$"AH{row}"].Value != null)
                                {
                                    employee.TrainingName = worksheet.Cells[$"AH{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"Training Name: {employee.TrainingName}";
                                }

                                if (worksheet.Cells[$"AI{row}"].Value != null)
                                {
                                    employee.TrainingRemark = worksheet.Cells[$"AI{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"Training Remark: {employee.TrainingRemark}";
                                }

                                if (worksheet.Cells[$"AJ{row}"].Value != null)
                                {
                                    DateTime dateTimeString;
                                    if (DateTime.TryParse(worksheet.Cells[$"AJ{row}"].Value.ToString(), out dateTimeString))
                                    {
                                        employee.TrainingDeliveryDate = dateTimeString;
                                    }
                                    else
                                    {
                                        employee.TrainingDeliveryDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"AJ{row}"].Value.ToString()));
                                    }
                                    value = $"Training Date : {employee.TrainingDeliveryDate}";
                                }

                                if (worksheet.Cells[$"AK{row}"].Value != null)
                                {
                                    employee.HasTraining = true;
                                    value = $"HasTraining: {employee.HasTraining}";
                                }

                                if (worksheet.Cells[$"AL{row}"].Value != null)
                                {
                                    employee.TrainingGrade = worksheet.Cells[$"AL{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"Training Grade: {employee.TrainingGrade}";
                                }

                                if (worksheet.Cells[$"AM{row}"].Value != null)
                                {
                                    employee.BpjsNumber = worksheet.Cells[$"AM{row}"].Value == null ? worksheet.Cells[$"AM{row}"].Value.ToString().Replace(" ", string.Empty) : null;
                                    value = $"BpjsNumber : {employee.BpjsNumber}";
                                }

                                if (worksheet.Cells[$"AN{row}"].Value != null)
                                {
                                    employee.BpjsRemark = worksheet.Cells[$"AN{row}"].Value == null ? worksheet.Cells[$"AN{row}"].Value.ToString() : null;
                                    value = $"BpjsRemark : {employee.BpjsRemark}";
                                }

                                if (worksheet.Cells[$"AP{row}"].Value != null)
                                {
                                    employee.JamsostekNumber = worksheet.Cells[$"AP{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"JamsostekNumber : {employee.JamsostekNumber}";
                                }

                                if (worksheet.Cells[$"AQ{row}"].Value != null)
                                {
                                    employee.JamsostekRemark = worksheet.Cells[$"AQ{row}"].Value.ToString();
                                    value = $"JamsostekRemark : {employee.JamsostekRemark}";
                                }
                                else
                                {
                                    employee.JamsostekRemark = null;
                                }

                                if (worksheet.Cells[$"AS{row}"].Value != null)
                                {
                                    employee.NPWP = worksheet.Cells[$"AS{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"NPWP : {employee.NPWP}";
                                }
                                else
                                {
                                    employee.NPWP = null;
                                }

                                if (worksheet.Cells[$"AT{row}"].Value != null)
                                {
                                    employee.KTP = worksheet.Cells[$"AT{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"KTP : {employee.KTP}";
                                }
                                else
                                {
                                    worksheet.Cells[$"AT{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"AV{row}"].Value != null)
                                {
                                    employee.AccountName = worksheet.Cells[$"AV{row}"].Value.ToString();
                                    value = $"AccountName : {employee.AccountName}";
                                }

                                if (worksheet.Cells[$"AW{row}"].Value != null)
                                {
                                    employee.BankCode = banks.Where(column => column.Code.ToLower().Replace(" ", string.Empty) == worksheet.Cells[$"AW{row}"].Value.ToString().ToLower().Replace(" ", string.Empty)).FirstOrDefault().Code;
                                    value = $"BankCode : {employee.BankCode}";
                                }

                                if (worksheet.Cells[$"AX{row}"].Value != null)
                                {
                                    employee.AccountNumber = worksheet.Cells[$"AX{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"AccountNumber : {employee.AccountNumber}";
                                }
                                else
                                {
                                    worksheet.Cells[$"AX{row}"].Value = "Belum Diisi";
                                    isValid = false;
                                }

                                if (worksheet.Cells[$"AY{row}"].Value != null)
                                {
                                    employee.KK = worksheet.Cells[$"AY{row}"].Value.ToString().Replace(" ", string.Empty);
                                    value = $"KK : {employee.KK}";
                                }

                                if (isValid)
                                {
                                    if (isExist)
                                    {
                                        payrollDB.Entry(employee).State = EntityState.Modified;
                                        updateEmployees.Add(employee);
                                    }
                                    else
                                    {
                                        employee.RoleId = 2;
                                        using (MD5 md5Hash = MD5.Create())
                                        {
                                            employee.Password = GetMd5Hash(md5Hash, employee.NIK.ToString());
                                        }
                                        payrollDB.Entry(employee).State = EntityState.Added;
                                        newEmployees.Add(employee);
                                    }

                                }
                                else
                                {
                                    isAllValid = false;
                                    return BadRequest($"Data baris ke {row}, pada kolom setelah {value} bermasalah");
                                }

                            }

                        }
                        payrollDB.Employee.UpdateRange(updateEmployees);
                        await payrollDB.Employee.AddRangeAsync(newEmployees);
                        await payrollDB.SaveChangesAsync();
                    }
                }
                return new JsonResult("OK");

            }
            catch (Exception error)
            {
                logger.LogError(error, $"Employee API - Create at row {row}");
                if (value == "")
                {
                    return new OkObjectResult(error.Message);
                }
                else
                {
                    return BadRequest($"Error occured at row {row} for column after {value}");
                }
                throw error;
            }
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
        [Route("api/employee/readDatatable")]
        public  async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                EmployeeView employeeView = new EmployeeView();
                employeeView.Data = await payrollDB.Employee
                    .Include(table => table.Location.District)
                    .Include(table => table.Position)
                    .Include(table => table.Customer)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Location.Name.Contains(request.Keyword) || column.Customer.Name.Contains(request.Keyword) || column.Position.Name.Contains(request.Keyword) || column.NIK.ToString().Contains(request.Keyword))
                    .OrderBy(column => column.Name)
                    .Skip(request.Skip)
                    .Take(request.PageSize)
                    .ToListAsync();
                employeeView.RecordsFiltered = await payrollDB.Employee
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return Ok(employeeView);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Employee API Controller - Read");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/employee/readDetail/{nik}")]
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
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/employee/update/{id}")]
        public async Task<IActionResult> Update([FromForm]EmployeeInput employeeInput, int id)
        {
            try
            {
                Employee employee = await payrollDB.Employee
                    .Where(column => column.NIK == id)
                    .FirstOrDefaultAsync();
                employee.Name = employeeInput.Name;
                employee.BirthPlace = employeeInput.BirthPlace;
                employee.BirthDate = employeeInput.BirthDate;
                employee.Sex = employeeInput.Sex;
                employee.Religion = employeeInput.Religion;
                employee.Address = employeeInput.Address;
                employee.PhoneNumber = employeeInput.PhoneNumber;
                employee.KTP = employeeInput.KTP;
                employee.KK = employeeInput.KK;
                employee.NPWP = employeeInput.NPWP;
                employee.JamsostekNumber = employeeInput.JamsostekNumber;
                employee.JamsostekRemark = employeeInput.JamsostekRemark;
                employee.BpjsNumber = employeeInput.BpjsNumber;
                employee.BpjsRemark = employeeInput.BpjsRemark;
                employee.DriverLicense = employeeInput.DriverLicense;
                employee.DriverLicenseType = employeeInput.DriverLicenseType;
                employee.DriverLicenseExpire = employeeInput.DriverLicenseExpire;
                employee.AccountNumber = employeeInput.AccountNumber;
                employee.AccountName = employeeInput.AccountName;
                employee.BankCode = employeeInput.BankCode;
                employee.FamilyStatusCode = employeeInput.FamilyStatusCode;
                employee.EmploymentStatusId = employeeInput.EmploymentStatusId;
                employee.PositionId = employeeInput.PositionId;
                employee.CustomerId = employeeInput.CustomerId;
                employee.LocationId = employeeInput.LocationId;
                employee.RoleId = employeeInput.RoleId;
                employee.StartContract = employeeInput.StartContract;
                employee.EndContract = employeeInput.EndContract;
                employee.JoinCompanyDate = employeeInput.JoinCompanyDate;
                employee.JoinCustomerDate = employeeInput.JoinCustomerDate;
                employee.HasUniform = employeeInput.HasUniform;
                employee.UniformDeliveryDate = employeeInput.UniformDeliveryDate;
                employee.HasIdCard = employeeInput.HasIdCard;
                employee.IdCardDeliveryDate = employeeInput.IdCardDeliveryDate;
                employee.HasTraining = employeeInput.HasTraining;
                employee.TrainingDeliveryDate = employeeInput.TrainingDeliveryDate;
                employee.TrainingName = employeeInput.TrainingName;
                employee.TrainingRemark = employeeInput.TrainingRemark;
                employee.TrainingGrade = employeeInput.TrainingGrade;
                employee.IsExist = employeeInput.IsExist;
                payrollDB.Entry(employee).State = EntityState.Modified;
                payrollDB.Employee.Update(employee);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(employee);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Employee API - Update {id}");
                throw error;
            }
        }

    }
}
