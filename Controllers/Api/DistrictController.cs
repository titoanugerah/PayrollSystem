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
        [HttpPost]
        [Route("api/district/create")]
        public async Task<IActionResult> Create([FromForm]DistrictInput districtInput)
        {
            try
            {
                District district = new District();
                district.Name = districtInput.Name;
                district.Remark = districtInput.Remark;
                district.IsExist = true;
                payrollDB.District.Add(district);                
                payrollDB.Entry(district).State = EntityState.Added;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Create");
                throw error;
            }
        }


        [Authorize]
        [HttpGet]
        [Route("api/district/read")]
        public async Task<IActionResult>Read()
        {
            try
            {
                List<District> district = await payrollDB.District
                    .ToListAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Read");
                throw error;
            }
        }


        [Authorize]
        [HttpPost]
        [Route("api/district/readDatatable")]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                DistrictView districtView = new DistrictView();
                districtView.Data = await payrollDB.District
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Remark.Contains(request.Keyword))
                    .Skip(request.Skip)
                    .OrderBy(column => column.Name)
                    .Take(request.PageSize)
                    .ToListAsync();
                districtView.RecordsFiltered = await payrollDB.District
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return new JsonResult(districtView);
            }
            catch(Exception error)
            {
                logger.LogError(error, $"District API - Read");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/district/readDeleted")]
        public async Task<IActionResult> ReadDeleted()
        {
            try
            {
                List<District> district = await payrollDB.District
                    .Where(column => column.IsExist == false)
                    .ToListAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Read Deleted");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/district/readDetail/{id}")]
        public async Task<IActionResult> ReadDetail(int id)
        {
            try
            {
                District district = payrollDB.District
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Read Detail {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/district/update/{id}")]
        public async Task<IActionResult> Update([FromForm]DistrictInput districtInput, int id)
        {
            try
            {
                District district = await payrollDB.District
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                district.Name = districtInput.Name;
                district.Remark = districtInput.Remark;
                payrollDB.Entry(district).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Update {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/district/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                District district = await payrollDB.District
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                district.IsExist = false;
                payrollDB.Entry(district).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Delete {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/district/recover/{id}")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                District district = await payrollDB.District
                    .Where(collumn => collumn.Id == id)
                    .FirstOrDefaultAsync();
                district.IsExist = true;
                payrollDB.Entry(district).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Recover {id}");
                throw error;
            }
        }
    }
}
