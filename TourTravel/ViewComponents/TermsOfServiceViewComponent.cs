using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class TermsOfServiceViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
