namespace LagerSystemApi.Models.Domain
{
    public class Log
    {
        public int id { get; set; }
        public string log_type { get; set; }
        public string log_message { get; set; }
        public DateTime timestamp { get; set; }
    }
}
