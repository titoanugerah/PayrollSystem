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

                string fileName = "NewEmployee.xlsx";
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, "file", fileName);
                System.IO.File.Delete(filePath);
                file.CopyTo(new FileStream(filePath, FileMode.Create));

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var excelPackage = new ExcelPackage(new FileInfo(filePath)))
                {
                    bool isAllValid = true;
                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {
                        ExcelCellAddress start = worksheet.Dimension.Start;
                        ExcelCellAddress end = worksheet.Dimension.End;
                        for (int row = 7; row <= end.Row; row++)
                        {
                            bool isValid = true;
                            Employee employee = new Employee();

                            if (worksheet.Cells[$"C{row}"].Value != null)
                            {
                                employee.NIK = int.Parse(worksheet.Cells[$"C{row}"].Value.ToString());
                            }
                            else
                            {
                                worksheet.Cells[$"C{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"D{row}"].Value != null)
                            {
                                employee.Name = worksheet.Cells[$"D{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"D{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"E{row}"].Value != null)
                            {
                                employee.PhoneNumber = worksheet.Cells[$"E{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"E{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"G{row}"].Value != null)
                            {
                                employee.PositionId = positions.Where(column => column.Name.ToLower() == worksheet.Cells[$"G{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
                            }
                            else if (worksheet.Cells[$"H{row}"].Value != null)
                            {
                                employee.PositionId = positions.Where(column => column.Name.ToLower() == worksheet.Cells[$"H{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
                            }
                            else if (worksheet.Cells[$"I{row}"].Value != null)
                            {
                                employee.PositionId = positions.Where(column => column.Name.ToLower() == worksheet.Cells[$"I{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
                            }
                            else if (worksheet.Cells[$"J{row}"].Value != null)
                            {
                                employee.PositionId = positions.Where(column => column.Name.ToLower() == worksheet.Cells[$"J{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
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
                                employee.LocationId = locations.Where(column => column.Name.ToLower() == worksheet.Cells[$"K{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
                            }
                            else
                            {
                                worksheet.Cells[$"K{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"L{row}"].Value != null)
                            {
                                employee.CustomerId = customers.Where(column => column.Name.ToLower() == worksheet.Cells[$"L{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
                            }
                            else
                            {
                                worksheet.Cells[$"L{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"N{row}"].Value != null)
                            {
                                employee.JoinCustomerDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"N{row}"].Value.ToString()));
                            }
                            else
                            {
                                worksheet.Cells[$"N{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"O{row}"].Value != null)
                            {
                                employee.Address = worksheet.Cells[$"O{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"O{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"P{row}"].Value != null)
                            {
                                employee.FamilyStatusCode = familyStatuses.Where(column => column.Code.ToLower() == worksheet.Cells[$"P{row}"].Value.ToString().ToLower()).FirstOrDefault().Code;
                            }
                            else
                            {
                                worksheet.Cells[$"P{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"Q{row}"].Value != null)
                            {
                                employee.Religion = worksheet.Cells[$"Q{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"Q{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"R{row}"].Value != null)
                            {
                                employee.Sex = worksheet.Cells[$"R{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"R{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"R{row}"].Value != null)
                            {
                                employee.Sex = worksheet.Cells[$"R{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"R{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"T{row}"].Value != null)
                            {
                                employee.BirthPlace = worksheet.Cells[$"T{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"T{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"U{row}"].Value != null)
                            {
                                employee.BirthDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"U{row}"].Value.ToString()));
                            }
                            else
                            {
                                worksheet.Cells[$"U{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"V{row}"].Value != null)
                            {
                                employee.StartContract = DateTime.FromOADate(int.Parse(worksheet.Cells[$"V{row}"].Value.ToString()));
                            }
                            else
                            {
                                worksheet.Cells[$"V{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"W{row}"].Value != null)
                            {
                                employee.EndContract = DateTime.FromOADate(int.Parse(worksheet.Cells[$"W{row}"].Value.ToString()));
                            }
                            else
                            {
                                worksheet.Cells[$"W{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"X{row}"].Value != null)
                            {
                                employee.EmploymentStatusId = employmentStatuses.Where(column => column.Name.ToLower() == worksheet.Cells[$"X{row}"].Value.ToString().ToLower()).FirstOrDefault().Id;
                            }
                            else
                            {
                                worksheet.Cells[$"X{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"AA{row}"].Value != null)
                            {
                                employee.DriverLicense = worksheet.Cells[$"AA{row}"].Value.ToString();
                            }
                            //else
                            //{
                            //    worksheet.Cells[$"AA{row}"].Value = "Belum Diisi";
                            //    isValid = false;
                            //}

                            if (worksheet.Cells[$"AB{row}"].Value != null)
                            {
                                employee.DriverLicenseExpire = DateTime.FromOADate(int.Parse(worksheet.Cells[$"AB{row}"].Value.ToString()));
                            }
                            //else
                            //{
                            //    worksheet.Cells[$"AB{row}"].Value = "Belum Diisi";
                            //    isValid = false;
                            //}

                            if (worksheet.Cells[$"AC{row}"].Value != null)
                            {
                                employee.HasUniform = true;
                            }
                            if (worksheet.Cells[$"AD{row}"].Value != null )
                            {
                                employee.HasIdCard = true;
                            }
                            if (worksheet.Cells[$"AE{row}"].Value != null )
                            {
                                employee.HasTraining = true;
                            }  
                            //else
                            //{
                            //    worksheet.Cells[$"AC{row}"].Value = "Belum Diisi";
                            //    worksheet.Cells[$"AD{row}"].Value = "Belum Diisi";
                            //    worksheet.Cells[$"AE{row}"].Value = "Belum Diisi";
                            //    isValid = false;
                            //}

                            if (worksheet.Cells[$"AG{row}"].Value != null)
                            {
                                employee.BpjsNumber = worksheet.Cells[$"AG{row}"].Value == null? worksheet.Cells[$"AG{row}"].Value.ToString():null;
                            }
                            //else
                            //{
                            //    worksheet.Cells[$"AG{row}"].Value = "Belum Diisi";
                            //    isValid = false;
                            //}

                            if (worksheet.Cells[$"AH{row}"].Value != null)
                            {
                                employee.BpjsRemark = worksheet.Cells[$"AH{row}"].Value == null ? worksheet.Cells[$"AH{row}"].Value.ToString() : null;
                            }
                            //else
                            //{
                            //    employee.BpjsRemark = "";
                            //}

                            if (worksheet.Cells[$"AI{row}"].Value != null)
                            {
                                employee.JamsostekNumber = worksheet.Cells[$"AI{row}"].Value.ToString();
                            }
                            //else
                            //{
                            //    worksheet.Cells[$"AI{row}"].Value = "Belum Diisi";
                            //    isValid = false;
                            //}

                            if (worksheet.Cells[$"AJ{row}"].Value != null)
                            {
                                employee.JamsostekRemark = worksheet.Cells[$"AJ{row}"].Value.ToString();
                            }
                            else
                            {
                                employee.JamsostekRemark = null;
                            }

                            if (worksheet.Cells[$"AL{row}"].Value != null)
                            {
                                employee.NPWP = worksheet.Cells[$"AL{row}"].Value.ToString();
                            }
                            else
                            {
                                employee.NPWP = null;
                            }

                            if (worksheet.Cells[$"AM{row}"].Value != null)
                            {
                                employee.KTP = worksheet.Cells[$"AM{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"AM{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"AO{row}"].Value != null)
                            {
                                employee.AccountName = worksheet.Cells[$"AO{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"AO{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"AP{row}"].Value != null)
                            {
                                employee.BankCode = banks.Where(column => column.Code.ToLower() == worksheet.Cells[$"AP{row}"].Value.ToString().ToLower()).FirstOrDefault().Code;
                            }
                            else
                            {
                                worksheet.Cells[$"AP{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"AQ{row}"].Value != null)
                            {
                                employee.AccountNumber = worksheet.Cells[$"AQ{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"AQ{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"AR{row}"].Value != null)
                            {
                                employee.KK = worksheet.Cells[$"AR{row}"].Value.ToString();
                            }
                            //else
                            //{
                            //    worksheet.Cells[$"AR{row}"].Value = "Belum Diisi";
                            //    isValid = false;
                            //}

                            if (worksheet.Cells[$"AX{row}"].Value != null)
                            {
                                employee.JoinCompanyDate = DateTime.FromOADate(int.Parse(worksheet.Cells[$"AX{row}"].Value.ToString()));
                            }
                            else
                            {
                                worksheet.Cells[$"AX{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (worksheet.Cells[$"AZ{row}"].Value != null)
                            {
                                employee.Email = worksheet.Cells[$"AZ{row}"].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[$"AZ{row}"].Value = "Belum Diisi";
                                isValid = false;
                            }

                            if (isValid)
                            {
                                if (employees.Where(column => column.NIK == employee.NIK).Count()>0)
                                {
                                    Employee currentEmployee = employees.Where(column => column.NIK == employee.NIK).FirstOrDefault();
                                    currentEmployee = employee;
                                }
                                else
                                {
                                    employee.IsExist = true;
                                    newEmployees.Add(employee);
                                }

                            }
                            else
                            {
                                isAllValid = false;
                                continue;
                            }

                        }

                    }
                    await payrollDB.Employee.AddRangeAsync(newEmployees);
                    await payrollDB.SaveChangesAsync();
                }
                return new JsonResult("OK");

            }
            catch (Exception error)
            {
                logger.LogError(error, $"Employee API - Create");
                throw error;
            }
        }

        [Authorize]
        [Route("api/employee/readDatatable")]
        public  async Task<IActionResult> ReadDatatable()
        {
            try
            {
                EmployeeView employeeView = new EmployeeView();
                employeeView.Data = await payrollDB.Employee
                    .Include(table => table.Location.District)
                    .Include(table => table.Position)
                    .Include(table => table.Customer)
                    .ToListAsync();
                return new JsonResult(employeeView);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Employee API Controller - Read");
                throw error;
            }
        }
    }
}
