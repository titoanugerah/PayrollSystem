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

        [HttpPost]
        [Route("api/district/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm]DistrictInput districtInput)
        {
            try
            {
                bool isExist = payrollDB.District
                    .Where(column => column.Name == districtInput.Name)
                    .Any();
                if (!isExist)
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
                else
                {
                    return BadRequest($"{districtInput.Name} sebelumnya sudah terdaftar");
                }

            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Create");
                return BadRequest(error.Message);
            }
        }


        [HttpGet]
        [Route("api/district/read")]
        [Authorize]
        public async Task<IActionResult>Read()
        {
            try
            {
                List<District> district = await payrollDB.District
                    .OrderBy(column => column.Name)
                    .ToListAsync();
                return new JsonResult(district);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"District API - Read");
                return BadRequest(error.Message);
            }
        }


        [HttpPost]
        [Route("api/district/readDatatable")]
        [Authorize]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                DistrictView districtView = new DistrictView();
                districtView.Data = await payrollDB.District
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Remark.Contains(request.Keyword))
                    .OrderBy(column => column.Name)
                    .Skip(request.Skip)
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
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/district/readDeleted")]
        [Authorize]
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
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Authorize]
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
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/district/update/{id}")]
        [Authorize(Roles = "Admin")]
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
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/district/delete/{id}")]
        [Authorize(Roles = "Admin")]
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
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/district/recover/{id}")]
        [Authorize(Roles ="Admin")]
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
                return BadRequest(error.Message);
            }
        }
    }
}
