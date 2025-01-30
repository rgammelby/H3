using LagerSystemApi.Models.Domain;

namespace LagerSystemApi.Models.DTO
{
    public class DeviceDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_archived { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public int location_id { get; set; }
        public string qr { get; set; }
    }
}
