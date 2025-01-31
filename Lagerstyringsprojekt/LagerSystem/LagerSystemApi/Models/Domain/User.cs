using System.ComponentModel.DataAnnotations.Schema;

namespace LagerSystemApi.Models.Domain
{
    [Table("User")]
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string hashedpwd { get; set; }
        public string salt { get; set; }
        public string telephone { get; set; }
        public bool is_active { get; set; }
        public string type { get; set; }
    }
}
