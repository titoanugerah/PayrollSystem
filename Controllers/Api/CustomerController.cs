using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using Payroll.ViewModels;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        [Route("api/customer/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm]CustomerInput customerInput)
        {
            try
            {
                bool isExist = payrollDB.Customer
                    .Where(column => column.Name == customerInput.Name)
                    .Any();
                if (!isExist)
                {
                    Customer customer = new Customer();
                    customer.Name = customerInput.Name;
                    customer.Remark = customerInput.Remark;
                    customer.MainCustomerId = customerInput.MainCustomerId;
                    customer.IsExist = true;
                    payrollDB.Customer.Add(customer);
                    payrollDB.Entry(customer).State = EntityState.Added;

                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(customer);
                }
                else
                {
                    return BadRequest($"{customerInput.Name} sebelumnya sudah terdaftar");
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer API Controller - Create");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/customer/readDatatable")]
        [Authorize]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                CustomerView customerView = new CustomerView();
                customerView.Data = await payrollDB.Customer
                    .Include(table => table.MainCustomer)
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Remark.Contains(request.Keyword) || column.MainCustomer.Name.Contains(request.Keyword))
                    .OrderBy(column => column.Name)
                    .Skip(request.Skip)
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
                return BadRequest(error.Message);
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
                    .Include(table => table.MainCustomer)
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                return new JsonResult(customer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Customer API Controller - Read Detail {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/customer/read")]
        [Authorize]
        public async Task<IActionResult> Read()
        {
            try
            {
                List<Customer> customers = await payrollDB.Customer
                    .Include(table => table.MainCustomer)
                    .OrderBy(column => column.Name)
                    .ToListAsync();
                return new JsonResult(customers);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Customer API - Read");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] CustomerInput customerInput, int id)
        {
            try
            {
                Customer customer = payrollDB.Customer.Where(column => column.Id == id).FirstOrDefault();
                customer.Name = customerInput.Name;
                customer.Remark = customerInput.Remark;
                customer.MainCustomerId = customerInput.MainCustomerId;
                payrollDB.Entry(customer).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(customer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Customer API Controller - Update {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/customer/delete/{id}")]
        [Authorize(Roles ="Admin")]
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
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/customer/recover/{id}")]
        [Authorize(Roles = "Admin")]
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
                return new JsonResult(customer);
            }
            catch(Exception error)
            {
                logger.LogError(error, $"Customer API - Recover {id}");
                return BadRequest(error.Message);
            }
        }
        
    }
}
