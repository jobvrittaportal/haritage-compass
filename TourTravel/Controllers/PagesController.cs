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
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "About Us");
            if(page != null)
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

        public IActionResult Team(int currentpage = 1)
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Team");
            ViewBag.CurrentPage = currentpage;
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

        public IActionResult TeamDetails(int id)
        {
       
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Single Team");
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

            var TeamMember = db.Team.FirstOrDefault(g => g.Id == id);
            if (TeamMember == null)
            {
                return NotFound();
            }
            
            return View(TeamMember);
        }

        public IActionResult Guide( int currentpage = 1)
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Guide");
            ViewBag.CurrentPage = currentpage;
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

        public IActionResult GuideDetails(int id)
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Guide Single");
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

            var guide = db.TourGuideView.FirstOrDefault(g => g.Id == id);
            if (guide == null)
            {
                return NotFound();
            }
            

            return View(guide);
        }

        public IActionResult Testimonials(bool showHeading)
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Testimonials");
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
            ViewData["ShowHeading"] = showHeading;
            return View();
        }

        public IActionResult Faq()
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "FAQs");
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

            var fAQ = db.FAQ.ToList();

            return View(fAQ);
        }
        public IActionResult Gallery(int currentpage = 1, int pageSize = 12)
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Gallery");
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
            var totalItems = db.Gallery.Count();

            var galleryItems = db.Gallery.OrderByDescending(g => g.Id).Skip((currentpage - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = currentpage;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);         
            return View(galleryItems);
        }

        public IActionResult PrivacyPolicy()
        {
            
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Privacy Policy");
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

        public IActionResult TermsOfService()
        {
           
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Terms Of Service");
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
        public IActionResult Error()
        {
          
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Error");
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
        public IActionResult ComingSoon()
        {

            var page = db.SitePages.FirstOrDefault(f => f.Page == "Coming Soon");
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
