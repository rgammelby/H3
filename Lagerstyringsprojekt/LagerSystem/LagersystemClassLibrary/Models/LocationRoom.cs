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
        

       // nav prop
        // public Location Location { get; set; }
        
        // Many-to-Many with Location
        public ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
