namespace TourTravel.Models
{
    public class TourCardsView : Base
    {
        public required string Title { get; set; }
        public string? ImageUrl { get; set; }
        public required string Duration { get; set; } // e.g., 7D/6N
        public required int MaxPerson { get; set; }
        public required string Location { get; set; }
        public required decimal Price { get; set; }
        public int? ReviewsCount { get; set; }
    }
}
