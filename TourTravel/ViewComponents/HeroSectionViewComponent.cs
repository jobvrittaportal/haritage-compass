using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class HeroSectionViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
