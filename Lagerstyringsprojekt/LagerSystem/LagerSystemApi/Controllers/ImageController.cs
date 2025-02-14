using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.Interfaces;

namespace LagerSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IImageService imageService, ILogger<ImageController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var (fileData, contentType) = await _imageService.GetImage(id);
                if (fileData == null) return NotFound("Image not found");

                //Returns the actual image file
                return File(fileData, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching image: {ex.Message}");
                return BadRequest("An error occurred while fetching the image.");
            }
        }
    }
}
