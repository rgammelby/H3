namespace LagerSystemApi.Models.DTO
{
    public class UpdateUserDTO
    {
        public int id {  get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string telephone { get; set; }
        public string password { get; set; }
    }
}
