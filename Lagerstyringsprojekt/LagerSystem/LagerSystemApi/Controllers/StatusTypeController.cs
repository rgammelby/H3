using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Interfaces;
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
    }
}
