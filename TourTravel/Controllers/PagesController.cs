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
            ViewBag.Page = "About Us";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "About");
            if(page != null)
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

        public IActionResult Team()
        {
            ViewBag.Page = "Team";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Team");
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

        public IActionResult TeamDetails(int id)
        {
            ViewBag.Page = "Team Details";

            var page = db.SitePages.FirstOrDefault(f => f.Page == "Team");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            var TeamMember = db.TourGuideView.FirstOrDefault(g => g.Id == id);
            if (TeamMember == null)
            {
                return NotFound();
            }
            var viewModel = new TourGuideDetail
            {
                TourGuide = TeamMember
            };

            return View(viewModel);
        }

        public IActionResult Guide(bool showHeading)
        {
            ViewBag.Page = "Guides";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Guides");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            ViewData["ShowHeading"] = showHeading;
            return View();
        }

        public IActionResult GuideDetails(int id)
        {
            ViewBag.Page = "Guide Detail";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Guides");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

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
            ViewBag.Page = "Testimonials";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Testimonials");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            ViewData["ShowHeading"] = showHeading;
            return View();
        }

        public IActionResult Faq()
        {
            ViewBag.Page = "Faq";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Faq");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            var fAQ = db.FAQ.ToList();

            return View(fAQ);
        }
        public IActionResult Gallery()
        {
            ViewBag.Page = "Gallery";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Gallery");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            var Gallery = db.Gallery.ToList();
            return View(Gallery);
        }

        public IActionResult PrivacyPolicy()
        {
            ViewBag.Page = "Privacy Policy";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Privacy Policy");
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

        public IActionResult TermsOfService()
        {
            ViewBag.Page = "Terms Of Service";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Terms Of Service");
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
