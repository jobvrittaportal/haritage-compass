using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using System.Linq;

namespace TourTravel.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly MyDbContext _db;

        public HeaderViewComponent(MyDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            // Get the latest WebsiteSetting record
            var companyDetails = _db.WebsiteSetting
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();

            // If not found, create a default one (to prevent null reference issues)
            if (companyDetails == null)
            {
                companyDetails = new WebsiteSetting
                {
                    Email = "info@example.com",
                    Phone = "+91 123456789000",
                    Address = "Your company address",
                    Facebook = "#",
                    Instagram = "#",
                    Twitter = "#",
                    LinkedIn = "#",
                    YouTube = "#",
                    Timing = "Mon–Fri: 10:00 AM – 8:00 PM",
                    RotationTime = 3
                };
            }

            return View(companyDetails); // ✅ Pass model to the view
        }
    }
}
