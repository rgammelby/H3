using LagerSystemApi.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO;

namespace LagerSystemApi.Services
{
    public class UploadImageService : IUploadImages
    {
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadImages");
        private readonly ILogger<UploadImageService> _logger;

        public UploadImageService(ILogger<UploadImageService> logger)
        {
            // Ensures that the folder exists before trying to save an image in it
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
                _logger.LogError($"Error while saving image: {file.FileName}.\nError: {ex.Message}");
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

                // Extract the file name from the path (avoid folder path issues)
                string fileName = Path.GetFileName(file);

                // Correct file path
                string filePath = Path.Combine(_uploadFolder, fileName);

                if (File.Exists(filePath))
                {
                    // Ensure the file is not locked before deleting
                    bool isFileFree = IsFileAccessible(filePath);
                    if (!isFileFree)
                    {
                        _logger.LogWarning($"File is locked: {filePath}");
                        return false;
                    }

                    // Wait a little to ensure any file system cache is cleared
                    await Task.Delay(100);

                    // Delete the file
                    File.Delete(filePath);
                    _logger.LogInformation($"Successfully deleted the image: {filePath}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"File not found: {filePath}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting image: {file}. Error: {ex.Message}");
                return false;
            }
        }

        // Helper function to check if the file is accessible
        private bool IsFileAccessible(string filePath)
        {
            try
            {
                // Try opening the file with exclusive access to check if it is locked
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                // File is locked or inaccessible
                return false;
            }
        }
    }
}
