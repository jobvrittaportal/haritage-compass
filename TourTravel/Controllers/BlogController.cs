using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
    public class BlogController(MyDbContext db) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Our Blogs";
            ViewBag.Page = "Blogs";
            return View();
        }
        public IActionResult SingleBlog(int id)
        {
            ViewBag.Title = "Blog Detail";
            ViewBag.Page = "Blog Detail";
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
