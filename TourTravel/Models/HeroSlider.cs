namespace TourTravel.Models
{
    public class HeroSlider : Base
    {
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public required string Description { get; set; }
        public  string? Image { get; set; }
        public required int RotationTime { get; set; }
    }
}
