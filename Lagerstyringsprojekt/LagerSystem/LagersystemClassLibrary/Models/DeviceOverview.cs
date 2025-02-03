using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class DeviceOverview
    {
        public DeviceOverview() { }

        [Key]
        public int id { get; set; }  // seq

        [Required]
        public string model { get; set; }  // changed to model
        public int device_type { get; set; }  // FK DeviceType
        public string image { get; set; }
        public int qty { get; set; }
        public int available_qty { get; set; }
        public DateTime last_ordered { get; set; }

        // Navigation property
        public DeviceType DeviceType { get; set; }

        // 1-m nav prop
        public ICollection<SingleDevice> Devices { get; set; } = new List<SingleDevice>();
    }
}
