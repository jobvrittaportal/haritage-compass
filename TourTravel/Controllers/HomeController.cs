using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _db;

        // ? Inject both ILogger and MyDbContext via dependency injection
        public HomeController(ILogger<HomeController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        public IActionResult Index()
        {
            ViewBag.Page = "Home";
            var page = _db.SitePages.FirstOrDefault(f => f.Page == "Home");
            if (page != null)
            {

                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
                //ViewBag.ImageType = page.ImageType;
            }
            var whyChooseUs = _db.WhyChooseUs.ToList();
            var heroSliders = _db.HeroSlider.ToList();
            var website = _db.WebsiteSetting.OrderByDescending(f => f.Id).FirstOrDefault();
            var RotationTime = website.RotationTime;
            var viewModel = new HomeViewModel
            {
                HeroSliders = heroSliders,
                RotationTime = RotationTime,
                WhyChooseUs = whyChooseUs
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
