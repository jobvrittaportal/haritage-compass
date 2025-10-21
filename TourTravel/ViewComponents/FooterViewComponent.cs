using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
    public class FooterViewComponent: ViewComponent
    {
        private readonly MyDbContext _db;

        public FooterViewComponent(MyDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
         
            var companyDetails = _db.WebsiteSetting.OrderByDescending(f => f.Id).FirstOrDefault();

            if (companyDetails == null)
            {
                companyDetails = new WebsiteSetting
                {
                    Email = "info@example.com",
                    Phone = "+91 123456789000000",
                    Address = "Your company address",
                    Facebook = "#",
                    Instagram = "#",
                    Twitter = "#",
                    LinkedIn = "#",
                    YouTube = "#",
                    Timing = "Mon–Sat: 9:00 AM – 6:00 PM"
                };
            }

            return View(companyDetails); // ✅ Pass model to the view
        }
    }
}
