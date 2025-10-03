namespace TourTravel.Models
{
    public class Blog : Base
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ImageUrl { get; set; }
        public required string Content { get; set; }
    }
}
