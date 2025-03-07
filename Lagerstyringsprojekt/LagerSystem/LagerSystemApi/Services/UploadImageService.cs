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
        public async Task<bool> DeleteImage(string file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                {
                    return false;
                }

                // Extract only the file name (avoiding folder path issues)
                string fileName = Path.GetFileName(file);

                // Correct file path
                string file_path = Path.Combine(_uploadFolder, fileName);

                if (File.Exists(file_path))
                {
                    // Ensure the file is not locked before deleting
                    bool isFileFree = IsFileAccessible(file_path);
                    if (!isFileFree)
                    {
                        return false;
                    }

                    await Task.Delay(100);
                    File.Delete(file_path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Helper function to check if file is accessible
        private bool IsFileAccessible(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false; // File is still locked
            }
        }
    }
}
