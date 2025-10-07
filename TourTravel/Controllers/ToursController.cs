using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class ToursController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Page = "Tour Package";
            ViewBag.Title = "Tour Package";
            return View();
        }
        public IActionResult TourPackageOffer()
        {
            ViewBag.Page = "Tour Offer";
            ViewBag.Title = "Tour Offer";
            return View();
        }
        public IActionResult TourPackageCart()
        {
            ViewBag.Page = "Tour Cart";
            ViewBag.Title = "Tour Cart";
            return View();
        }
        public IActionResult TourPackageBookings()
        {
            ViewBag.Page = "Tour Booking";
            ViewBag.Title = "Tour Booking";
            return View();
        }
        public IActionResult TourPackageConfirm()
        {
            ViewBag.Page = "Booking Confirm";
            ViewBag.Title = "Tour Confirm";
            return View();
        }
        public IActionResult TourPackageSearch()
        {
            ViewBag.Page = "Tour Search";
            ViewBag.Title = "20 Result Found";
            return View();
        }
        public IActionResult TourPackageDetails()
        {
            ViewBag.Page = "Tour Detail";
            ViewBag.Title = "Tour Detail";
            return View();
        }
    }
}
