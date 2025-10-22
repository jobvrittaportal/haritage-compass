namespace TourTravel.Models
{
    public class SitePages : Base
    {
        public required string Page { get; set; }
        public string? Image { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string KeyWords { get; set; }
        public required int ImgWidth { get; set; }
        public required int ImgHeight { get; set; }
    }
}
