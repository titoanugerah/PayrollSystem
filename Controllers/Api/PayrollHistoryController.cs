using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/payrollHistory/create")]
        public async Task<IActionResult> Create([FromForm]PayrollInput payrollInput)
        {
            try
            {
                bool isExist = payrollDB.PayrollHistory
                    .Where(column => column.MainCustomerId == payrollInput.MainCustomerId)
                    .Where(column => column.Month == payrollInput.Month)
                    .Where(column => column.Year == payrollInput.Year)
                    .Any();
                if (!isExist)
                {
                    PayrollHistory payrollHistory = new PayrollHistory();
                    payrollHistory.MainCustomerId = payrollInput.MainCustomerId;
                    if (payrollInput.MainCustomerId == 1)
                    {
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
                    }
                    else if (payrollInput.MainCustomerId == 2)
                    {

                    }
                    else if (payrollInput.MainCustomerId == 3)
                    {
                        payrollHistory.JamsostekPercentage = payrollConfiguration.Value.JamsostekPercentage;
                        payrollHistory.BpjsPercentage = payrollConfiguration.Value.BpjsPercentage;
                        payrollHistory.PensionPercentage = payrollConfiguration.Value.PensionPercentage;
                        payrollHistory.ManagementFeePercentage = 0;
                        payrollHistory.PpnPercentage = 0;
                        payrollHistory.BpjsTk1Percentage = payrollConfiguration.Value.BpjsTk1Percentage;
                        payrollHistory.BpjsKesehatanPercentage = payrollConfiguration.Value.BpjsKesehatanPercentage;
                        payrollHistory.Pension1Percentage = payrollConfiguration.Value.Pension1Percentage;
                        payrollHistory.Pph21Percentage = payrollConfiguration.Value.PPH21Percentage;
                        payrollHistory.Pph23Percentage = payrollConfiguration.Value.PPH23Percentage;
                        payrollHistory.BpjsPayrollPercentage = payrollConfiguration.Value.BpjsPayrollPercentage;
                        payrollHistory.PensionPayrollPercentage = payrollConfiguration.Value.PensionPayrollPercentage;
                        payrollHistory.Month = payrollInput.Month;
                        payrollHistory.Year = payrollInput.Year;
                        payrollHistory.StatusId = 1;
                        payrollHistory.IsExist = true;
                        payrollDB.Entry(payrollHistory).State = EntityState.Added;
                        await payrollDB.PayrollHistory.AddAsync(payrollHistory);
                    }
                    else if (payrollInput.MainCustomerId == 4)
                    {

                    }

                    await payrollDB.SaveChangesAsync();

                    List<Employee> employees = await payrollDB.Employee
                        .Where(column => column.MainCustomerId == payrollInput.MainCustomerId)
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
                return BadRequest(error.Message);
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
                return BadRequest(error.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/payrollHistory/check/{id}")]
        public async Task<IActionResult> Check(int id)
        {
            try
            {
                int notYetUploaded = payrollDB.PayrollDetail
                    .Include(table => table.Employee)
                    .Where(column => column.PayrollHistoryId == id)
                    .Where(column => column.PayrollDetailStatusId == 1)
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Employee.IsExist == true)
                    .Count();
                bool isAlreadySubmitted = payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .Where(column => column.StatusId == 3)
                    .Any();
                    
                if (notYetUploaded > 0)
                {
                    return BadRequest($"Terdapat {notYetUploaded} pekerja yang belum di upload data gajinya");
                }
                else if (isAlreadySubmitted)
                {
                    return BadRequest($"Gajian periode ini sudah di submit");
                }
                else
                {
                    return new JsonResult(Ok());
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll History API - Check {id}");
                return BadRequest(error.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/payrollHistory/readDatatable/{id}")]
        public async Task<IActionResult> ReadDatatable(int id)
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                PayrollHistoryView payrollHistoryView = new PayrollHistoryView();
                payrollHistoryView.Data = await payrollDB.PayrollHistory
                    .Where(column => column.IsExist == true)
                    .Where(column => column.MainCustomerId == id)
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
        public async Task<IActionResult> Submit(int id, [FromForm] int isLateTransfer)
        {
            try
            {
                PayrollHistory payrollHistory = await payrollDB.PayrollHistory
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                List<PayrollDetail> payrollDetails = await payrollDB.PayrollDetail
                    .Include(table => table.Employee)
                    .Where(column => column.PayrollHistoryId == id)
                    .ToListAsync();
                payrollHistory.StatusId = 3;
                payrollDB.Entry(payrollHistory).State = EntityState.Modified;
                payrollDB.Update(payrollHistory);
                await payrollDB.SaveChangesAsync();
                foreach (PayrollDetail payrollDetail in payrollDetails)
                {
                    if (payrollDetail.Employee.BankCode == "BCA" & isLateTransfer == 1)
                    {
                        payrollDetail.TransferFee = 5000;
                    }
                    payrollDetail.PayrollDetailStatusId = 3;
                    payrollDB.Entry(payrollDetail).State = EntityState.Modified;
                }
                payrollDB.UpdateRange(payrollDetails);
                await payrollDB.SaveChangesAsync();

                return new JsonResult(Ok());
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Payroll History API - Submit {id}");
                throw error;
            }
        }
    }
}
