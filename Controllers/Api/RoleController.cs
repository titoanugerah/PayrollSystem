using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.DataAccess;
using Payroll.Models;

namespace Payroll.Controllers.Api
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> logger;
        private readonly PayrollDB payrollDB;

        public RoleController(ILogger<RoleController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Authorize]
        [HttpGet]
        [Route("api/role/read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                List<Role> roles = await payrollDB.Role
                    .OrderBy(column => column.Name)
                    .ToListAsync();
                return new JsonResult(roles);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Role API - Read");
                throw error;
            }
        }
    }
}
