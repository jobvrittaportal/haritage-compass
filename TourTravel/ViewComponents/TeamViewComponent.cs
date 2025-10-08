using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class TeamViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
