using Microsoft.AspNetCore.Mvc;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class PrivacyPolicyViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var privacyPolicy = db.PrivacyPolicy.ToList();
            var viewmodel = new PrivacyPolicy
            {
                PrivacyPolicies = privacyPolicy
            };
            return View(viewmodel);
        }
    }
}
