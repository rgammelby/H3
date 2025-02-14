using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.Services
{
    public class ImageService: IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<(byte[] FileData, string ContentType)> GetImage(int id)
        {
            if (id <= 0) throw new Exception("Id cannot be 0 or less");

            string imagePath = await _imageRepository.GetImage(id);
            if (string.IsNullOrEmpty(imagePath)) return (null, null);

            // Convert relative path to absolute path
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), imagePath.TrimStart('/'));

            if (!File.Exists(fullPath)) return (null, null);

            byte[] fileBytes = await File.ReadAllBytesAsync(fullPath);
            string contentType = GetContentType(fullPath);

            return (fileBytes, contentType);
        }

        // Gets the images extension type
        private string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };
        }
    }
}
