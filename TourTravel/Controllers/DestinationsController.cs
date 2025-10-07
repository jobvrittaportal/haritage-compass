using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class DestinationsController : Controller
    {
        public IActionResult Index(bool showHeading )
        {

            ViewBag.Title = "Destination Detail";
            ViewBag.Page = "Destination Detail";
            ViewData["ShowHeading"] = showHeading;
            return View();
        }
        public IActionResult DestinationDetails()
        {
            ViewBag.Title = "Destination Detail";
            ViewBag.Page = "Destination Detail";
            return View();
        }
    }
}
