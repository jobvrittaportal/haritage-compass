using Microsoft.AspNetCore.Mvc;
using TourTravel.Data;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class TestimonialsViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(int take = 6)
        {
            var testimonials = db.Testimonial.OrderByDescending(t => t.Id).Take(take).ToList();

            var viewModel = new TestimonialViewModel
            {
                Testimonials = testimonials,
                ShowHeading = true // or false depending on logic
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
