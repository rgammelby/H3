using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("DeviceType")]
    public class DeviceType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int qty { get; set; }
        public string img { get; set; }
    }
}
