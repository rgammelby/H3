using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("LocationRoom")]
    public class LocationRoom
    {
        public int id { get; set; }
        public string designation { get; set; }
        public int location_cupboard { get; set; }
    }
}
