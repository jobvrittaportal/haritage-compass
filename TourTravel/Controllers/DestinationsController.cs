using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class DestinationsController(MyDbContext db) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Page = "Destinations";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
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
        public IActionResult DestinationDetails(int Id)
        {
            ViewBag.Page = "Destination Detail";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            var destination = db.Destinations.FirstOrDefault(d => d.Id == Id);
            if (destination == null) return NotFound();

            var galleryImages = db.DestinationGallery.Where(g => g.DestinationId == Id).ToList();

            var viewModel = new TourTravel.ViewModel.DestinationDetailsViewModel
            {
                Destination = destination,
                GalleryImages = galleryImages
            };

            return View(viewModel);
        }
    }
}
