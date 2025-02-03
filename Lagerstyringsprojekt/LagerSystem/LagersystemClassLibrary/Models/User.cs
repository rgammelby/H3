using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary.Models
{
    public class User
    {
        public User() { }

        [Key]
        public int id { get; set; }

        [Required]
        public string first_name { get; set; }

        [Required]
        public string last_name { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string telephone { get; set; }
        public bool is_active { get; set; } = true;

        [Required]
        public string type { get; set; } = "user";

        [Required]
        public string password { get; set; }

        [Required]
        public string salt { get; set; }

        // Navigation Property for one-to-many relationship
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
