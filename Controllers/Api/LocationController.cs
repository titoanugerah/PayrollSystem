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
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> logger;
        private readonly PayrollDB payrollDB;
        public LocationController(ILogger<LocationController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("api/location/create")]
        public async Task<IActionResult> Create([FromForm]LocationInput locationInput)
        {
            try
            {
                bool isExist = payrollDB.Location
                    .Where(column => column.Name == locationInput.Name)
                    .Any();
                if (!isExist)
                {
                    Location location = new Location();
                    location.Name = locationInput.Name;
                    location.UMK = locationInput.UMK;
                    location.DistrictId = locationInput.DistrictId;
                    location.IsExist = true;
                    payrollDB.Location.Add(location);
                    payrollDB.Entry(location).State = EntityState.Added;
                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(location);
                }
                else
                {
                    return BadRequest($"{locationInput.Name} sebelumnya sudah terdaftar");
                }

            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Create");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/location/readDatatable")]
        [Authorize]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                LocationView locationView = new LocationView();
                locationView.Data = await payrollDB.Location
                    .Include(table => table.District)
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.District.Name.Contains(request.Keyword))
                    .OrderBy(column => column.Name)
                    .Skip(request.Skip)                    
                    .Take(request.PageSize)
                    .ToListAsync();
                locationView.RecordsFiltered = await payrollDB.Location
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return new JsonResult(locationView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Read");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/location/read")]
        [Authorize]
        public async Task<IActionResult> Read()
        {
            try
            {
                List<Location> locations = await payrollDB.Location
                    .OrderBy(column => column.Name)
                    .ToListAsync();
                return new JsonResult(locations);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Read");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/location/readDeleted")]
        [Authorize]
        public async Task<IActionResult> ReadDeleted()
        {
            try
            {
                List<Location> location = await payrollDB.Location
                    .Where(column => column.IsExist == false)
                    .ToListAsync();
                return new JsonResult(location);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Read Delted");
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("api/location/readDetail/{id}")]
        [Authorize]
        public async Task<IActionResult> ReadDetail(int id)
        {
            try
            {
                Location location = await payrollDB.Location
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                return new JsonResult(location);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Read Detail {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/location/update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm]LocationInput locationInput, int id)
        {
            try
            {
                Location location = await payrollDB.Location
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                location.Name = locationInput.Name;
                location.UMK = locationInput.UMK;
                location.DistrictId = locationInput.DistrictId;
                payrollDB.Entry(location).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(location);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Update {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/location/delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Location location = await payrollDB.Location
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                location.IsExist = false;
                payrollDB.Entry(location).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(location);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Delete {id}");
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("api/location/recover/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            { 
                Location location = await payrollDB.Location
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                location.IsExist = true;
                payrollDB.Entry(location).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(location);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Recover {id}");
                return BadRequest(error.Message);
            }
        }
    }
}
