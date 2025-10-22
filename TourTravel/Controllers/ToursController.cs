using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class ToursController(MyDbContext db) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Page = "Tour Package";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            return View();
        }
        public IActionResult TourPackageOffer()
        {
            ViewBag.Page = "Tour Offer";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Offer");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult TourPackageCart()
        {
            ViewBag.Page = "Tour Cart";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Cart");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult TourPackageBookings()
        {
            ViewBag.Page = "Tour Booking";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Booking");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult TourPackageConfirm()
        {
            ViewBag.Page = "Booking Confirm";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Booking Confirm");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult TourPackageSearch()
        {
            ViewBag.Page = "Tour Search";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Search");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult TourPackageDetails()
        {
            ViewBag.Page = "Tour Detail";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Detail");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
    }
}
