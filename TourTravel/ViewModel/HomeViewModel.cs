using Org.BouncyCastle.Utilities;
using TourTravel.Models;

namespace TourTravel.ViewModel
{
    public class HomeViewModel
    {
        public List<HeroSlider> HeroSliders { get; set; } = new();
        public int RotationTime { get; set; }
        public List<WhyChooseUs> WhyChooseUs { get; set; } = new();
        public required IntroVideo IntroVideo { get; set; }

    }
}
