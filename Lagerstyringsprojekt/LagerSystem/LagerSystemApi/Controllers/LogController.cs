using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Interfaces;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                var logs = await _logService.GetAllLogs();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            //catch (NotFoundException ex) // Handle when no logs exist
            //{
            //    return NotFound(new { message = ex.Message });
            //}
            //catch (DatabaseException ex) // Handle database errors
            //{
            //    return StatusCode(500, new { message = "A database error occurred.", details = ex.Message });
            //}
            //catch (Exception ex) // Handle unexpected errors
            //{
            //    return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            //}
        }
    }
}
