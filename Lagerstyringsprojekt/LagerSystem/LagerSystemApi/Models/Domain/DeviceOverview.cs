using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("DeviceOverview")]
    public class DeviceOverview
    {
        public int id { get; set; }
        public int device_type { get; set; }
        public string model { get; set; }
        public int available_qty { get; set; }
        public int qty { get; set; }
        public string image { get; set; }
        public DateTime last_ordered {  get; set; }
    }
}
