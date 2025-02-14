using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LagerstyringClassLibrary.Models
{
    public class Log 
    {
        public Log() { }

        [Key]
        public int id { get; set; }
        
        [Required]
        public string log_type { get; set; }
        
        [Required]
        public string log_message { get; set; }
        public DateTime timestamp { get; set; }
    }
}
