namespace LagerSystemApi.Models.DTO
{
    public class UserDTO
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string telephone { get; set; }
        public string is_active { get; set; }
        public string type { get; set; }

    }
}
