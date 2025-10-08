namespace TourTravel.ViewModel
{
    public class TestimonialViewModel 
    {
        public IEnumerable<TourTravel.Models.Testimonial> Testimonials { get; set; } = new List<TourTravel.Models.Testimonial>();
        public bool ShowHeading { get; set; } = true;
    }
}
