using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class Activity
    {
        public Activity() { }

        [Key]
        public int id { get; set; }
        public int device_id { get; set; }  // FK SingleDevice ID
        public int activity_type { get; set; }  // FK ActivityType ID
        public int user_id { get; set; }  // FK User ID
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_on { get; set; }
        public string notes { get; set; }

        [Required]
        public Guid lifecycle_id { get; set; }  // changed to guid


        // Navigation Properties
        public ActivityType ActivityType { get; set; }

        public SingleDevice SingleDevice { get; set; }

        public User User { get; set; }

    }
}
