using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Interfaces;
using LagerSystemApi.Services;
namespace LagerSystemApi.Controllers
{
    public class StatusTypeController : ControllerBase
    {
        public interface IStatusTypeController
        {
            Task<IActionResult> GetAllStatusTypes();
        }

        private readonly IStatusTypeService _statusTypeService;

        public StatusTypeController(IStatusTypeService statusTypeService)
        {
            _statusTypeService = statusTypeService;
        }

        [HttpGet("GetAllStatusTypes")]
        public async Task<IActionResult> GetAllStatusTypes()
        {
            try
            {
                var statusTypes = await _statusTypeService.GetAllStatusTypes();
                if (statusTypes == null)
                {
                    return BadRequest(new { message = "No status types found." });
                }
                return Ok(statusTypes);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting all status types. ");
            }
        }

        [HttpGet("GetStatusTypeById/{id:int}")]
        public async Task<IActionResult> GetStatusTypeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Invalid Status Type id." });
                }

                var statusType = await _statusTypeService.GetStatusTypeById(id);
                if (statusType == null)
                {
                    return NotFound(new { message = $"Status Type with id {id} not found." });
                }
                return Ok(statusType);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest($"Error getting status type by id: {id}");
            }
        }
    }
}
