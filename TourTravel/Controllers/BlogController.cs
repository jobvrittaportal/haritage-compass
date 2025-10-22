using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
    public class BlogController(MyDbContext db) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Page = "Blogs";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Blogs");
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
        public IActionResult SingleBlog(int id)
        {
            ViewBag.Page = "Blog Detail";
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Blogs");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            var blog = db.Blog.FirstOrDefault(b => b.Id == id);
            var Recentblogs = db.Blog.OrderByDescending(b => b.Id).Take(3).ToList();
            if (blog == null)
            {
                return NotFound();
            }
            var viewModel = new SingleBlogViewModel
            {
                Blog = blog,
                RecentBlogs = Recentblogs
            };
            return View(viewModel);
        }
       
    }
}
