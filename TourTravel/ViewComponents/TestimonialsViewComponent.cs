using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class TestimonialsViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(bool showheading, int take)
        {
            var testimonials = db.Testimonial.OrderByDescending(t => t.Id).Take(take).ToList();

            var viewModel = new TestimonialViewModel
            {
                Testimonials = testimonials,
                ShowHeading = showheading // or false depending on logic
            };

            return View(viewModel);
        }
    }

    //public class TestimonialsViewComponent : ViewComponent
    //{
    //    public IViewComponentResult Invoke()
    //    {
    //        return View();
    //    }
    //}
}
