using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class SingleDevice
    {
        public SingleDevice()
        {

        }

        [Key]
        public int id { get; set; }  // sequential
        //public string model { get; set; }  // changed to model  // string value of type via DeviceOverview  // removed; duplicate data
        //public string device_type { get; set; }  // string value of type via DeviceOverview  // removed; duplicate data

        public int status { get; set; }  // FK id from StatusType

        // Foreign Key for LocationCupboard
        [ForeignKey("Location")]
        public int location { get; set; }  // FK id from LocationCupboard  location = $"{Location.Cupboard + Cupboard.roomid} where id = {id}";
        public int device_overview_id { get; set; }  // FK DeviceOverview
        public string description { get; set; }
        public string qr { get; set; }
        public bool is_archived { get; set; } = false;

        // Navigation Property for one-to-many relationship
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        // Navigation properties
        public StatusType Statuses { get; set; }
        //public Location Locations { get; set; }
        public DeviceOverview DeviceOverview { get; set; }

        public LocationCupboard Location { get; set; }
    }
}
