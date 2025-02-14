using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("LocationCupboard")]
    public class LocationCupboard
    {
        public int id { get; set; }
        public string designation { get; set; }
    }
}
