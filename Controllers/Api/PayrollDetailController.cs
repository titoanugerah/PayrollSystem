using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class PayrollDetailController : ControllerBase
    {
        private readonly ILogger<PayrollDetailController> logger;
        private readonly PayrollDB payrollDB;

        public PayrollDetailController(ILogger<PayrollDetailController> _logger, PayrollDB _payrollDB)
        {
            payrollDB = _payrollDB;
            logger = _logger;
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollDetail/readDatatable/{id}")]
        public async Task<IActionResult> ReadDatatable(int id)
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                PayrollDetailView payrollDetailView = new PayrollDetailView();
                payrollDetailView.Data = await payrollDB.PayrollDetail
//                    .Include(table => table.Employee)
                    //.Where(column => column.PayrollHistoryId == id)
                    .Where(column => column.Employee.Name.Contains(request.Keyword) || column.PayrollDetailStatus.Contains(request.Keyword))
                    .Skip(request.Skip)
                    .OrderBy(column => column.Employee.Name)
                    .Take(request.PageSize)
                    .ToListAsync();
                return new JsonResult(payrollDetailView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll Detail API - Read Datatable");
                throw error;
            }
        
        }
    }
}
