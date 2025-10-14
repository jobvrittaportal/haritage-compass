using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
    public class PagesController(MyDbContext db) : Controller
    {
        private readonly MyDbContext db = db;
        public IActionResult AboutUs()
        {
            ViewBag.Title = "About Us";
            ViewBag.Page = "About Us";
            return View();
        }

        public IActionResult Team()
        {
            ViewBag.Title = "Team";
            ViewBag.Page = "Team";
            return View();
        }

        public IActionResult TeamDetails()
        {
            ViewBag.Title = "Team Details";
            ViewBag.Page = "Team Details";
            return View();
        }

        public IActionResult Guide(bool showHeading)
        {

            ViewBag.Title = "Guides";
            ViewBag.Page = "Guides";
            ViewData["ShowHeading"] = showHeading;
            return View();
        }

        public IActionResult GuideDetails(int id)
        {
            ViewBag.Title = "Guide Detail";
            ViewBag.Page = "Guide Detail";
            var guide = db.TourGuideView.FirstOrDefault(g => g.Id == id);
            if (guide == null)
            {
                return NotFound();
            }
            var viewModel = new TourGuideDetail
            {
                TourGuide = guide
            };

            return View(viewModel);
        }

        public IActionResult Testimonials(bool showHeading)
        {
            ViewBag.Title = "Testimonials";
            ViewBag.Page = "Testimonials";
            ViewData["ShowHeading"] = showHeading;
            return View();
        }

        public IActionResult Faq()
        {
            ViewBag.Title = "FAQ";
            ViewBag.Page = "Faq";
            var fAQ = db.FAQ.ToList();

            return View(fAQ);
        }
        public IActionResult Gallery()
        {
            ViewBag.Title = "Gallery";
            ViewBag.Page = "Gallery";
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            ViewBag.Title = "Privacy Policy";
            ViewBag.Page = "Privacy Policy";
            return View();
        }

        public IActionResult TermsOfService()
        {
            ViewBag.Title = "Terms Of Service";
            ViewBag.Page = "Terms Of Service";
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.Title = "Login";
            ViewBag.Page = "Login";
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.Title = "Register";
            ViewBag.Page = "Register";
            return View();
        }
        public IActionResult ForgotPassword()
        {
            ViewBag.Title = "Forgot Password";
            ViewBag.Page = "Forgot Password";
            return View();
        }
    }
}
