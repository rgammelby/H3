using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class LocationRoom 
    {
        public LocationRoom() { }

        [Key]
        public int id { get; set; }

        [Required]
        public string designation { get; set; }


        // 1-m navigation property
        public ICollection<LocationCupboard> Cupboards { get; set; } = new List<LocationCupboard>();
    }
}
