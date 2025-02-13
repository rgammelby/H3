using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LagerSystemApi.Models.DTO
{
    public class AddDeviceOverviewDTO
    {
        [Required]
        public string model { get; set; }
        [Required]
        public int device_type { get; set; }
        public IFormFile image { get; set; }
        [JsonIgnore]
        public string? image_path { get; set; }
        public int? qty { get; set; }
        public int? available_qty { get; set; }
        [Required]
        public DateTime last_ordered { get; set; }
    }
}
