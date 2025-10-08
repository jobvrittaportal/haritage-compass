
using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Contact Us";
            ViewBag.Page = "Contact Us";
            return View();
        }
    }
}
