using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.ViewModels;
using System;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly ILogger<PayrollController> logger;
        private readonly PayrollDB payrollDB;
        public PayrollController(ILogger<PayrollController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

//        [Authorize]
//        [HttpPost]
//        [Route("api/payroll/create")]
//        public async Task<IActionResult> Create(PayrollInput payrollInput)
//        {
//            try
//            {
////                Payroll 
//            }
//            catch (Exception  error)
//            {
//                logger.LogError(error, "Payroll API - Create");
//                throw error;
//            }
//        }
    }
}
