namespace LagerSystemApi.Models.Domain
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public int activity { get; set; }
        public int type { get; set; }
    }
}
