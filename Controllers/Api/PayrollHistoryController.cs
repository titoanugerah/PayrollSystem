﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<PayrollConfiguration> payrollConfiguration;
        public PayrollHistoryController(ILogger<PayrollHistoryController> _logger, PayrollDB _payrollDB, IOptions<PayrollConfiguration> _payrollConfiguration)
        {
            logger = _logger;
            payrollDB = _payrollDB;
            payrollConfiguration = _payrollConfiguration;
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollHistory/create")]
        public async Task<IActionResult> Create([FromForm]PayrollInput payrollInput)
        {
            try
            {
                bool isExist = payrollDB.PayrollHistory
                    .Where(column => column.Month == payrollInput.Month)
                    .Where(column => column.Year == payrollInput.Year)
                    .Any();
                if (!isExist)
                {
                    PayrollHistory payrollHistory = new PayrollHistory();
                    payrollHistory.JamsostekPercentage = payrollConfiguration.Value.JamsostekPercentage;
                    payrollHistory.BpjsPercentage = payrollConfiguration.Value.BpjsPercentage;
                    payrollHistory.PensionPercentage = payrollConfiguration.Value.PensionPercentage;
                    payrollHistory.ManagementFeePercentage = payrollConfiguration.Value.ManagementFeePercentage;
                    payrollHistory.PpnPercentage = payrollConfiguration.Value.PpnPercentage;
                    payrollHistory.BpjsTk1Percentage = payrollConfiguration.Value.BpjsTk1Percentage;
                    payrollHistory.BpjsKesehatanPercentage = payrollConfiguration.Value.BpjsKesehatanPercentage;
                    payrollHistory.Pension1Percentage = payrollConfiguration.Value.Pension1Percentage;
                    payrollHistory.Pph21Percentage = payrollConfiguration.Value.PPH21Percentage;
                    payrollHistory.Pph23Percentage = payrollConfiguration.Value.PPH23Percentage;
                    payrollHistory.BpjsPayrollPercentage= payrollConfiguration.Value.BpjsPayrollPercentage;
                    payrollHistory.PensionPayrollPercentage = payrollConfiguration.Value.PensionPayrollPercentage;
                    payrollHistory.Month = payrollInput.Month;
                    payrollHistory.Year = payrollInput.Year;
                    payrollHistory.StatusId = 1;
                    payrollHistory.IsExist = true;
                    payrollDB.Entry(payrollHistory).State = EntityState.Added;
                    await payrollDB.PayrollHistory.AddAsync(payrollHistory);
                    await payrollDB.SaveChangesAsync();

                    List<Employee> employees = await payrollDB.Employee
                        .Where(column => column.IsExist == true)
                        .ToListAsync();
                    List<PayrollDetail> payrollDetails = new List<PayrollDetail>();
                    foreach (Employee employee in employees)
                    {
                        PayrollDetail payrollDetail = new PayrollDetail();
                        payrollDetail.EmployeeId = employee.NIK;
                        payrollDetail.PayrollHistoryId = payrollHistory.Id;
                        payrollDetail.MainPrice = 0;
                        payrollDetail.PayrollDetailStatusId = 1;
                        payrollDetails.Add(payrollDetail);
                        payrollDB.Entry(payrollDetail).State = EntityState.Added;
                    }
                    await payrollDB.PayrollDetail.AddRangeAsync(payrollDetails);
                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(payrollHistory);
                }
                else
                {
                    return BadRequest($"Periode {payrollInput.Month} {payrollInput.Year} sudah ada");
                }
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
                    .OrderBy(column => column.Month)
                    .Skip(request.Skip)
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

        [HttpPost]
        [Authorize]
        [Route("api/payrollHistory/submit/{id}")]
        public async Task<IActionResult> Submit(int id)
        {
            try
            {
                PayrollHistory payrollHistory = await payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                List<PayrollDetail> payrollDetails = await payrollDB.PayrollDetail
                    .Where(column => column.PayrollHistoryId == id)
                    .ToListAsync();
                bool isAnyUnUploaded = payrollDetails
                    .Where(column => column.PayrollDetailStatusId == 1)
                    .Any();
                if (!isAnyUnUploaded)
                {
                    payrollHistory.StatusId = 3;
                    payrollDB.Entry(payrollHistory).State = EntityState.Modified;
                    payrollDB.Update(payrollHistory);
                    await payrollDB.SaveChangesAsync();
                    foreach (PayrollDetail payrollDetail in payrollDetails)
                    {
                        payrollDetail.PayrollDetailStatusId = 3;
                        payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                    }
                    payrollDB.UpdateRange(payrollDetails);
                    await payrollDB.SaveChangesAsync();

                    return new JsonResult(Ok());
                }
                else
                {
                    return new JsonResult(BadRequest("semua belum terupload"));
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll History API - Submit {id}");
                throw error;
            }
        }
    }
}
