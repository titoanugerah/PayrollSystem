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

        [Authorize]
        [HttpGet]
        [Route("api/location/readDatatable")]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                LocationView locationView = new LocationView();
                locationView.Data = await payrollDB.Location
                    .Include(table => table.District)
                    .Where(column => column.IsExist == true)
                    .ToListAsync();
                return new JsonResult(locationView);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Location API - Read");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/location/readDeleted")]
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
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/location/readDetail/{id}")]
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
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/location/update/{id}")]
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
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/location/delete/{id}")]
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
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/location/recover/{id}")]
        public async Task<IActionResult> Recover(int id)
        {
            Location location = await payrollDB.Location
                .Where(column => column.Id == id)
                .FirstOrDefaultAsync();
            location.IsExist = true;
            payrollDB.Entry(location).State = EntityState.Modified;
            await payrollDB.SaveChangesAsync();
            return new JsonResult(location);
        }
    }
}
