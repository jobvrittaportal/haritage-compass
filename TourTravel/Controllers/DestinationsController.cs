using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class DestinationsController(MyDbContext db) : Controller
    {
        public IActionResult Index()
        {

            ViewBag.Title = "Destinations";
            ViewBag.Page = "Destinations";
            return View();
        }
        public IActionResult DestinationDetails(int Id)
        {
            ViewBag.Title = "Destination Detail";
            ViewBag.Page = "Destination Detail";
            var destination = db.Destinations.FirstOrDefault(d => d.Id == Id);
            if (destination == null)
            {
                return NotFound();
            }

            var galleryImages = db.DestinationGallery
                                  .Where(g => g.DestinationId == Id)
                                  .ToList();

            var viewModel = new TourTravel.ViewModel.DestinationDetailsViewModel
            {
                Destination = destination,
                GalleryImages = galleryImages
            };

            return View(viewModel);
        }
    }
}
