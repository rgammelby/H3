//using Microsoft.EntityFrameworkCore;
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
//        public int designation { get; set; }  // FK LocationRoom  Room 1 Cupboard 1
        
//        // 1-m navigation prop
//        ICollection<LocationRoom> Rooms { get; set; }

//        // nav prop
//        public SingleDevice SingleDevice { get; set; }

//    }
//}
