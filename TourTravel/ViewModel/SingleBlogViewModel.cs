using TourTravel.Models;

namespace TourTravel.ViewModel
{
    public class SingleBlogViewModel
    {
        public required Blog Blog { get; set; }
        public required List<Blog> RecentBlogs { get; set; }
        public required WebsiteSetting CompanyDetails { get; set; }
    
    }
}
