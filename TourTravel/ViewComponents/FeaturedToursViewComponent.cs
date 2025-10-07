using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class FeaturedToursViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
