using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
