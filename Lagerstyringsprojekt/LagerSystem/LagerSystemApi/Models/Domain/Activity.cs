using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("Activity")]
    public class Activity
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int device_id { get; set; }
        public int activity_type { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_at { get; set; }
        public string notes { get; set; }
        public int lifecycle_id { get; set; }
    }
}
