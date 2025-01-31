using System.ComponentModel.DataAnnotations;

namespace LagerSystemApi.Models.DTO
{
    public class AddSingleDeviceDTO
    {
        [Required]
        public int device_type { get; set; }
        [Required]
        public string model {  get; set; }
        [Required]
        public int available_qty { get; set; }
        [Required]
        public int qty { get; set; }
        public string image {  get; set; }
        [Required]
        public DateTime last_ordered {  get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public int status_id {  get; set; }
        [Required]
        public int location_id { get; set; }
        [Required]
        public string qr {  get; set; }
        [Required]
        public bool is_archived { get; set; }
    }
}
