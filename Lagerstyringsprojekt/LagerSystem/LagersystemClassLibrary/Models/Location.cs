//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class Location
    {
        public Location() { }

        [Key]
        public int id { get; set; }
        public int room_id { get; set; }
        public int cupboard_id { get; set; }

        // 1-m navigation prop
        public ICollection<LocationRoom> Rooms { get; set; }
        public ICollection<LocationCupboard> Cupboards { get; set; }

        // nav prop
        public ICollection<SingleDevice> Devices { get; set; } = new List<SingleDevice>();

    }
}
