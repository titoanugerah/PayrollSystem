using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.ViewModels;
using System;
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
                customerView.Data = await payrollDB.Customer.ToListAsync();
                return new JsonResult(customerView);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Customer Controller - Index");
                throw error;
            }

}
    }
}
