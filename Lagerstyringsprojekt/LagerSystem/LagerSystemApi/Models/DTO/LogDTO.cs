namespace LagerSystemApi.Models.DTO
{
    public class LogDTO
    {
        public int id { get; set; }
        public string log_type { get; set; }
        public string log_message { get; set; }
        public DateTime timestamp { get; set; }
    }
}
