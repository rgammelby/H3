using LagerSystemApi.Interfaces;

namespace LagerSystemApi.Services
{
    public class UploadImageService : IUploadImages
    {
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadImages");
        private readonly ILogger<UploadImageService> _logger;

        public UploadImageService(ILogger<UploadImageService> logger)
        {
            // Ensures that the folder exist before tryign to save a image in it
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
            _logger = logger;
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            if (file == null) return null;
            try
            {
                // Generate a unique file name
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(_uploadFolder, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate a relative path for database storage
                var relativePath = $"/UploadImages/{fileName}";
                return relativePath;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while saving image: {file.FileName}.\nError: {ex.Message}");
                return null;
            }
        }
    }
}
