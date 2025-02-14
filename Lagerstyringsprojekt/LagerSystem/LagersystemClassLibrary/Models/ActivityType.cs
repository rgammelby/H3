using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class ActivityType
    {
        public ActivityType() { }

        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(16)]
        public string activity_type { get; set; }


        // Navigation Property for one-to-many relationship
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
