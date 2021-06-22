using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly PayrollDB payrollDB;

        public EmployeeController(ILogger<EmployeeController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
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
                    .Select(column => new ViewModels.Employee 
                    { 
                      NIK = column.NIK, 
                      Name = column.Name,
                      Location = column.Location.Name,
                      Position = column.Position.Name,
                      Customer = column.Customer.Name,
                    }).ToListAsync();
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
