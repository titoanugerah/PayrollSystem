using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using Payroll.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> logger;
        private readonly PayrollDB payrollDB;
        public CustomerController(ILogger<CustomerController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Route("api/customer/readDatatable")]
        [Authorize]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                CustomerView customerView = new CustomerView();
                customerView.Data = await payrollDB.Customer
                    .Where(column => column.IsExist == true)
                    .ToListAsync();
                return new JsonResult(customerView);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer API Controller - Read Datatable");
                throw error;
            }

        }

        [Route("api/customer/readDeleted")]
        [Authorize]
        public async Task<IActionResult> readDeleted()
        {
            try
            {
                var deletedCustomer = await payrollDB.Customer
                    .Where(column => column.IsExist == false)
                    .Select(column => new
                    {
                        column.Id,
                        column.Name
                    })
                    .ToListAsync();
                return new JsonResult(deletedCustomer);

            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer API Controller - Read Deleted");
                throw error;
            }
        }


        [Route("api/customer/create")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> create(CreateCustomer createCustomer)
        {
            try
            {
//                Customer customer = new Customer(createCustomer);
                
//                var x = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault();
                return  Ok();
            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer API Controller - Create");
                throw error;
            }
        }
        
    }
}
