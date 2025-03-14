namespace LagerSystemApi.Models.DTO
{
    public class AddActivityDTO
    {
        public int device_id { get; set; }  // FK SingleDevice ID
        public int activity_type { get; set; }  // FK ActivityType ID
        public int user_id { get; set; }  // FK User ID
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_on { get; set; }
        public string notes { get; set; }


        public Guid lifecycle_id { get; set; }  // changed to guid
    }
}
