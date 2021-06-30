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
    public class PayrollHistoryController : ControllerBase
    {
        private readonly ILogger<PayrollHistoryController> logger;
        private readonly PayrollDB payrollDB;
        public PayrollHistoryController(ILogger<PayrollHistoryController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollHistory/create")]
        public async Task<IActionResult> Create(PayrollInput payrollInput)
        {
            try
            {
                PayrollHistory payrollHistory = new PayrollHistory();
                payrollHistory.Month = payrollInput.Month;

                payrollDB.Entry(payrollHistory).State = EntityState.Added;
                await payrollDB.PayrollHistory.AddAsync(payrollHistory);
                await payrollDB.SaveChangesAsync();
                return new JsonResult(payrollHistory);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll API - Create");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/payrollHistory/read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                List<PayrollHistory> payrollHistory = await payrollDB.PayrollHistory
                    .ToListAsync();
                return new JsonResult(payrollHistory);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll API - Read");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollHistory/readDatatable")]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                PayrollHistoryView payrollHistoryView = new PayrollHistoryView();
                payrollHistoryView.Data = await payrollDB.PayrollHistory
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Month.Contains(request.Keyword) || column.Year.Contains(request.Keyword))
                    .Skip(request.Skip)
                    .OrderBy(column => column.Month)
                    .Take(request.PageSize)
                    .ToListAsync();
                payrollHistoryView.RecordsFiltered = await payrollDB.PayrollHistory
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return new JsonResult(payrollHistoryView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll API - ReadDatatable");
                throw error;
            }
        }
    }
}
