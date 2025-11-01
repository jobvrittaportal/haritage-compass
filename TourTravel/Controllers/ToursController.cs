using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class ToursController(MyDbContext db) : Controller
    {
        public IActionResult Index()
        {
          
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package");
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

            return View();
        }
        public IActionResult TourPackageOffer()
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package Offer");
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
            return View();
        }
        public IActionResult TourPackageCart()
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Cart");
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
            return View();
        }
        public IActionResult TourPackageBookings()
        {
           
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package Booking");
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
            return View();
        }
        public IActionResult TourPackageConfirm()
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Booking Confirm");
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
            return View();
        }
        public IActionResult TourPackageSearch()
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Search");
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
            return View();
        }
        public IActionResult TourPackageDetails(int id)
        {
           
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Details");
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
            var packagedetail = db.TourCardsView.FirstOrDefault(x => x.Id == id);
            return View(packagedetail);
        }
    }
}
