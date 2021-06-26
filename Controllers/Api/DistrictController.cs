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
    public class DistrictController : ControllerBase
    {
        private readonly ILogger<DistrictController> logger;
        private readonly PayrollDB payrollDB;
        public DistrictController(ILogger<DistrictController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Authorize]
        [HttpGet]
        [Route("api/district/readDatatable")]
        public async Task<IActionResult> Read()
        {
            try
            {
                DistrictView districtView = new DistrictView();
                districtView.Data = await payrollDB.District
                    .Where(column => column.IsExist == true)
                    .ToListAsync();
                return new JsonResult(districtView);
            }
            catch(Exception error)
            {
                logger.LogError(error, $"District API - Read");
                throw error;
            }
        }
    }
}
