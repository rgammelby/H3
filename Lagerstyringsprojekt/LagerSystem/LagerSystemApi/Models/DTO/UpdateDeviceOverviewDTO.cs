namespace LagerSystemApi.Models.DTO
{
    public class UpdateDeviceOverviewDTO
    {
        public int id { get; set; }
        public string model { get; set; }
        public int device_type { get; set; }
        public string image { get; set; }
        public int qty { get; set; }
        public int available_qty { get; set; }
        public DateTime last_ordered { get; set; }
    }
}
