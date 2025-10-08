using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Our Blogs";
            ViewBag.Page = "Blogs";
            return View();
        }
        public IActionResult SingleBlog()
        {
            ViewBag.Title = "Blog Detail";
            ViewBag.Page = "Blog Detail";
            return View();
        }
    }
}
