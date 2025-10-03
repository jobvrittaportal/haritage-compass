namespace TourTravel.Models
{
    public class Testimonial : Base
    {
        public required string AuthorName { get; set; }
        public required string Role { get; set; }
        public required string Quote { get; set; }
        public string? ImageUrl { get; set; }
        public int? Rating { get; set; } = 5;
    }
}
