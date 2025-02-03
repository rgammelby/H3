using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class LocationCupboard 
    {
        public LocationCupboard() { }

        [Key]
        public int id { get; set; }
        [Required]
        public string designation { get; set; }

        // nav prop
        public LocationRoom Room { get; set; }
    }
}
