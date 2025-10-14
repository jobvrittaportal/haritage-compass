using TourTravel.Models;

namespace TourTravel.ViewModel
{
    public class TourGuideViewModel
    {
        public IEnumerable<TourTravel.Models.TourGuideView> TourGuides { get; set; } = new List<TourTravel.Models.TourGuideView>();
        public bool ShowHeading { get; set; } = true;
    }
    public class TourGuideDetail
    {
       public  required TourGuideView TourGuide { get; set; }

    }

}
