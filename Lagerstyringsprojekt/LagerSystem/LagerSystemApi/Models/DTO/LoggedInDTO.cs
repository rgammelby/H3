namespace LagerSystemApi.Models.DTO
{
    public class LoggedInDTO
    {
        public string token { get; set; }
        public string message { get; set; }
        public int status_code { get; set; }
    }
}
