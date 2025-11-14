using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
  public class BookNowController(MyDbContext db) : Controller
  {
        private readonly MyDbContext db = db;
        public IActionResult Index()
    {
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Book Now");
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
  }
}
