namespace TourTravel.Models
{
    public class TourGuideView : Base
    {
        public required string Name { get; set; }
        public required string Role { get; set; } = "Tour Guide";
        public string? ImageUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
    }
}
