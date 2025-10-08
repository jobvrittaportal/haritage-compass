namespace TourTravel.ViewModel
{
    public class BlogViewModel
    {
        public IEnumerable<TourTravel.Models.Blog> Blogs { get; set; } = new List<TourTravel.Models.Blog>();
        public bool ShowHeading { get; set; } = true;
    }
}
