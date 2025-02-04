using System.ComponentModel.DataAnnotations;

namespace LagerSystemApi.Models.DTO
{
    public class AddSingleDeviceDTO
    {
        [Required]
        public int status { get; set; }
        [Required]
        public int location { get; set; }
        [Required]
        public int device_overview_id { get; set; }  


        public string description { get; set; }
        public string qr { get; set; }
        public bool is_archived { get; set; } = false;

       
    }
}
