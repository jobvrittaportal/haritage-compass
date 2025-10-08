using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class PrivacyPolicyViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
