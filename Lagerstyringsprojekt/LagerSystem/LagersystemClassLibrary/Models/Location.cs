////using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LagerstyringClassLibrary.Models
//{
//    public class Location
//    {
//        public Location() { }

//        [Key]
//        public int id { get; set; }
//        // no need fks anymore:
//        //[ForeignKey("Room")]
//        //public int room_id { get; set; }
//        //[ForeignKey("Cupboard")]
//        //public int cupboard_id { get; set; }
        
//        // Navigation Properties (Many-to-Many)
//        public ICollection<LocationRoom> Rooms { get; set; } = new List<LocationRoom>();
//        public ICollection<LocationCupboard> Cupboards { get; set; } =  new List<LocationCupboard>();
        
//        // nav prop: devices in this lcoation:
//        public ICollection<SingleDevice> Devices { get; set; } = new List<SingleDevice>();
//    }
//}
