using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("StatusType")]
    public class StatusType
    {
        public int id { get; set; }
        public int status_type { get; set; }
    }
}
