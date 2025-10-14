using Microsoft.AspNetCore.Mvc;
using TourTravel.Migrations;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class TermsOfServiceViewComponent(MyDbContext db) : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
           var Terms = db.TermsOfService.ToList();
            var viewmodel = new TermOfServiceViewModel
            {
                TermsOfServices = Terms
            };
            return View(viewmodel);
        }
    }
}
