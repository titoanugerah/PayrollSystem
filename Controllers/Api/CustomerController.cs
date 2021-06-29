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

        [Route("api/customer/create")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CustomerInput customerInput)
        {
            try
            {
                Customer customer = new Customer();
                customer.Name = customerInput.Name;
                customer.Remark = customerInput.Remark;
                customer.IsExist = true;    
                payrollDB.Customer.Add(customer);
                payrollDB.Entry(customer).State = EntityState.Added;

                await payrollDB.SaveChangesAsync();
                return new JsonResult(customer);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer API Controller - Create");
                throw error;
            }
        }

        [Route("api/customer/readDatatable")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                CustomerView customerView = new CustomerView();
                customerView.Data = await payrollDB.Customer
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Remark.Contains(request.Keyword))
                    .Skip(request.Skip)
                    .OrderBy(column => column.Name)
                    .Take(request.PageSize)
                    .ToListAsync();
                customerView.RecordsFiltered = await payrollDB.Customer
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return new JsonResult(customerView);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer API Controller - Read Datatable");
                throw error;
            }
        }

        [HttpGet]
        [Route("api/customer/readdetail/{id}")]
        [Authorize]
        public async Task<IActionResult> ReadDetail(int id)
        {
            try
            {
                Customer customer = await payrollDB.Customer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                return new JsonResult(customer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Customer API Controller - Read Detail {id}");
                throw error;
            }
        }

        [Route("api/customer/readDeleted")]
        [Authorize]
        public async Task<IActionResult> ReadDeleted()
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

        [HttpPost]
        [Route("api/customer/update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] CustomerInput customerClientInput, int id)
        {
            try
            {
                Customer customer = payrollDB.Customer.Where(column => column.Id == id).FirstOrDefault();
                customer.Name = customerClientInput.Name;
                customer.Remark = customerClientInput.Remark;
                payrollDB.Entry(customer).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(customer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Customer API Controller - Update {id}");
                throw error;
            }
        }

        [HttpPost]
        [Route("api/customer/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Customer customer = await payrollDB.Customer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                customer.IsExist = false;
                payrollDB.Entry(customer).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(customer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Customer API - Delete {id}");
                throw error;
            }
        }

        [HttpPost]
        [Route("api/customer/recover/{id}")]
        [Authorize]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                Customer customer = await payrollDB.Customer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                customer.IsExist = true;
                payrollDB.Entry(customer).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return Ok();
            }
            catch(Exception error)
            {
                logger.LogError(error, $"Customer API - Recover {id}");
                throw error;
            }
        }
        
    }
}
