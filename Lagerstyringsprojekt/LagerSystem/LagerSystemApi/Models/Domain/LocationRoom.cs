namespace LagerSystemApi.Models.Domain
{
    public class LocationRoom
    {
        public int id { get; set; }
        public string designation { get; set; }
        public LocationCupboard locationCupboard { get; set; }
    }
}
