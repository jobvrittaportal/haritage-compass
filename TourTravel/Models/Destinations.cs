namespace TourTravel.Models
{
    public class Destinations : Base
    {
        public required string DestinationName { get; set; }
        public string? DestinationInfo { get; set; }
        public required string DestinationImgUrl { get; set; }
        public int? StartingPrice { get; set; }
        public string? MapUrl { get; set; }
    }
}