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
    public class MainCustomerController : ControllerBase
    {
        private readonly ILogger<MainCustomerController> logger;
        private readonly PayrollDB payrollDB;

        public MainCustomerController(ILogger<MainCustomerController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [HttpPost]
        [Route("api/maincustomer/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] MainCustomerInput mainCustomerInput)
        {
            try
            {
                bool isAny = await payrollDB.MainCustomer
                    .Where(column => column.Name == mainCustomerInput.Name)
                    .AnyAsync();
                if (!isAny)
                {
                    MainCustomer mainCustomer = new MainCustomer();
                    mainCustomer.Name = mainCustomerInput.Name;
                    mainCustomer.Remark = mainCustomerInput.Remark;
                    mainCustomer.IsExist = true;
                    payrollDB.Entry(mainCustomer).State = EntityState.Added;
                    await payrollDB.MainCustomer.AddAsync(mainCustomer);
                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(mainCustomer);
                }
                else
                {
                    return BadRequest($"Customer utama dengan nama {mainCustomerInput.Name} sudah ada");
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Create");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/maincustomer/read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                List<MainCustomer> mainCustomers = await payrollDB.MainCustomer
                    .ToListAsync();
                return new JsonResult(mainCustomers);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Read");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/maincustomer/readdatatable")]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                MainCustomerView mainCustomerView = new MainCustomerView();
                mainCustomerView.Data = await payrollDB.MainCustomer
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Remark.Contains(request.Keyword))
                    .OrderBy(column => column.Name)
                    .Skip(request.Skip)
                    .Take(request.PageSize)
                    .ToListAsync();
                mainCustomerView.RecordsFiltered = await payrollDB.MainCustomer
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return new JsonResult(mainCustomerView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Read Datatable");
                return BadRequest(error.Message);
                throw;
            }

        }

        [HttpGet]
        [Route("api/maincustomer/readdetail/{id}")]
        public async Task<IActionResult> ReadDetail(int id)
        {
            try
            {
                MainCustomer mainCustomer = await payrollDB.MainCustomer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                return new JsonResult(mainCustomer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Read Detail {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/maincustomer/readdeleted")]
        public async Task<IActionResult> ReadDeleted()
        {
            try
            {
                List<MainCustomer> mainCustomers = await payrollDB.MainCustomer
                    .Where(column => column.IsExist == false)
                    .ToListAsync();
                return new JsonResult(mainCustomers);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Read Deleted");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/maincustomer/update/{id}")]
        public async Task<IActionResult> Update([FromForm] MainCustomerInput mainCustomerInput, int id)
        {
            try
            {
                MainCustomer mainCustomer = await payrollDB.MainCustomer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                mainCustomer.Name = mainCustomerInput.Name;
                mainCustomer.Remark = mainCustomerInput.Remark;
                payrollDB.Entry(mainCustomer).State = EntityState.Modified;
                payrollDB.MainCustomer.Update(mainCustomer);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(mainCustomer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Update {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/maincustomer/delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                MainCustomer mainCustomer = await payrollDB.MainCustomer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                mainCustomer.IsExist = false;
                payrollDB.Entry(mainCustomer).State = EntityState.Modified;
                payrollDB.MainCustomer.Update(mainCustomer);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(mainCustomer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Delete {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/maincustomer/recover/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                MainCustomer mainCustomer = await payrollDB.MainCustomer
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                mainCustomer.IsExist = true;
                payrollDB.Entry(mainCustomer).State = EntityState.Modified;
                payrollDB.MainCustomer.Update(mainCustomer);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(mainCustomer);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Main Customer API - Recover {id}");
                return BadRequest(error.Message);
            }
        }
    }
}
