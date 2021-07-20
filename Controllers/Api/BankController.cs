using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class BankController : ControllerBase
    {
        public readonly ILogger<BankController> logger;
        public readonly PayrollDB payrollDB;

        public BankController(ILogger<BankController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Authorize]
        [HttpGet]
        [Route("api/bank/read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                List<Bank> banks = await payrollDB.Bank
                    .OrderBy(column => column.Name)
                    .ToListAsync();
                return new JsonResult(banks);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Bank API - Read");
                throw error;
            }
        }
    }
}
