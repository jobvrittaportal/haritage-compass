
using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Controllers
{
    public class ContactController : Controller
    {
        private readonly MyDbContext db;
        private readonly string _baseUrl;
        private readonly ApiUrlOptions _apiOptions;

        public ContactController(MyDbContext db, ApiUrlOptions apiOptions)
        {
            this.db = db;
            _apiOptions = apiOptions;

            // ⭐ Same Base URL logic as ToursController
            _baseUrl = apiOptions.Use switch
            {
                "Live" => apiOptions.Live,
                "Stage" => apiOptions.Stage,
                "Local" => apiOptions.Local,
                _ => apiOptions.Live
            };
        }

        public IActionResult Index()
        {

            var page = db.SitePages.FirstOrDefault(f => f.Page == "Contact Us");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            ViewBag.Baseurl = _baseUrl;
            var companyDetails = db.WebsiteSetting.OrderByDescending(f => f.Id).FirstOrDefault();

            // If not found, create a default one
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
                    Timing = "Mon–Sat: 9:00 AM – 6:00 PM",
                    RotationTime = 3
                };
            }
            return View(companyDetails);
        }
    }
}
