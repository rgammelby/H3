using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("ActivityType")]
    public class ActivityType
    {
        public int id { get; set; }
        public int activty_type { get; set; }
    }
}
