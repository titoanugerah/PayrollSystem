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
    public class PositionController : ControllerBase
    {
        private readonly ILogger<PositionController> logger;
        private readonly PayrollDB payrollDB;
        public PositionController(ILogger<PositionController> _logger, PayrollDB _payrollDB)
        {
            logger = _logger;
            payrollDB = _payrollDB;
        }

        [Authorize]
        [HttpPost]
        [Route("api/position/create")]
        public async Task<IActionResult> Create([FromForm]PositionInput positionInput)
        {
            try
            {
                bool isExist = payrollDB.Position
                    .Where(column => column.Name == positionInput.Name)
                    .Any();
                if (!isExist)
                {
                    Position position = new Position();
                    position.Name = positionInput.Name;
                    position.Remark = positionInput.Remark;
                    position.IsExist = true;
                    payrollDB.Position.Add(position);                
                    payrollDB.Entry(position).State = EntityState.Added;
                    await payrollDB.SaveChangesAsync();
                    return new JsonResult(position);
                }
                else
                {
                    return BadRequest($"{positionInput.Name} sebelumnya sudah terdaftar");
                }

            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Create");
                throw error;
            }
        }


        [Authorize]
        [HttpGet]
        [Route("api/position/read")]
        public async Task<IActionResult>Read()
        {
            try
            {
                List<Position> position = await payrollDB.Position
                    .ToListAsync();
                return new JsonResult(position);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Read");
                throw error;
            }
        }


        [Authorize]
        [HttpPost]
        [Route("api/position/readDatatable")]
        public async Task<IActionResult> ReadDatatable()
        {
            try
            {
                DatatablesRequest request = new DatatablesRequest(Request.Form.Select(column => new InputRequest { Key = column.Key, Value = column.Value }).ToList());
                PositionView positionView = new PositionView();
                positionView.Data = await payrollDB.Position
                    .Where(column => column.IsExist == true)
                    .Where(column => column.Name.Contains(request.Keyword) || column.Remark.Contains(request.Keyword))
                    .OrderBy(column => column.Name)
                    .Skip(request.Skip)
                    .Take(request.PageSize)
                    .ToListAsync();
                positionView.RecordsFiltered = await payrollDB.Position
                    .Where(column => column.IsExist == true)
                    .CountAsync();
                return new JsonResult(positionView);
            }
            catch(Exception error)
            {
                logger.LogError(error, $"Position API - Read");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/position/readDeleted")]
        public async Task<IActionResult> ReadDeleted()
        {
            try
            {
                List<Position> position = await payrollDB.Position
                    .Where(column => column.IsExist == false)
                    .ToListAsync();
                return new JsonResult(position);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Read Deleted");
                throw error;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/position/readDetail/{id}")]
        public async Task<IActionResult> ReadDetail(int id)
        {
            try
            {
                Position position = payrollDB.Position
                    .Where(column => column.Id == id)
                    .FirstOrDefault();
                return new JsonResult(position);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Read Detail {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/position/update/{id}")]
        public async Task<IActionResult> Update([FromForm]PositionInput positionInput, int id)
        {
            try
            {
                Position position = await payrollDB.Position
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                position.Name = positionInput.Name;
                position.Remark = positionInput.Remark;
                payrollDB.Entry(position).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(position);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Update {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/position/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Position position = await payrollDB.Position
                    .Where(column => column.Id == id)
                    .FirstOrDefaultAsync();
                position.IsExist = false;
                payrollDB.Entry(position).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(position);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Delete {id}");
                throw error;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/position/recover/{id}")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                Position position = await payrollDB.Position
                    .Where(collumn => collumn.Id == id)
                    .FirstOrDefaultAsync();
                position.IsExist = true;
                payrollDB.Entry(position).State = EntityState.Modified;
                await payrollDB.SaveChangesAsync();
                return new JsonResult(position);
            }
            catch (Exception error)
            {
                logger.LogError(error, $"Position API - Recover {id}");
                throw error;
            }
        }
    }
}
