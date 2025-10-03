using Microsoft.AspNetCore.Mvc;
using TourTravel.Data;

namespace TourTravel.ViewComponents
{
    //public class TestimonialsViewComponent(MyDbContext db): ViewComponent
    //{
    //    public IViewComponentResult Invoke(int take = 4)
    //    {
    //        var testimonials = db.Testimonial
    //            .OrderByDescending(t => t.Id)
    //            .Take(take)
    //            .ToList();

    //        return View(testimonials);
    //    }
    //}
    public class TestimonialsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
