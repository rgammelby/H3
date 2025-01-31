using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("Location")]
    public class Location
    {
        public int id { get; set; }
        public int room_id { get; set; }
    }
}
