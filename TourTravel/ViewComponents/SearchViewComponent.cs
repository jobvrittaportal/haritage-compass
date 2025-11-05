using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
