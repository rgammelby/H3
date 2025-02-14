using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("SingleDevice")]
    public class SingleDevice
    {
        public int id { get; set; }
        public int deviceOverview_id { get; set; }
        public bool is_archived { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public int location_id { get; set; }
        public string qr { get; set; }

    }
}
