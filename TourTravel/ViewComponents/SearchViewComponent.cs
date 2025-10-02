using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        // Optional: you can pass data to the view via parameters
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
