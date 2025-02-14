using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class StatusType 
    {
        public StatusType() { }

        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(16)]
        public string status_type { get; set; }

        // 1-m navigation prop
        public ICollection<SingleDevice> Devices { get; set; } = new List<SingleDevice>();

    }
}
