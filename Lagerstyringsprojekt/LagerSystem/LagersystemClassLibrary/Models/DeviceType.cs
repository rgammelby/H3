using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class DeviceType
    {
        public DeviceType() { }

        [Key]
        public int id { get; set; }
        [Required]
        public string type_name { get; set; }

        // 1-m navigation property
        public ICollection<DeviceOverview> Overviews { get; set; } = new List<DeviceOverview>();
    }
}
