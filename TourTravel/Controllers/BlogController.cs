using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
    public class BlogController(MyDbContext db) : Controller
    {
        public IActionResult Index(int currentpage = 1, string? search = null)
        {
      
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Blogs");          
            ViewBag.CurrentPage = currentpage;
            ViewBag.Search = search;
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
        public IActionResult SingleBlog(int id)
        {
           
            var page = db.SitePages.FirstOrDefault(f => f.Page == "Single Blog");
            var companyDetails = db.WebsiteSetting.OrderByDescending(f => f.Id).FirstOrDefault();
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

            var blog = db.Blog.FirstOrDefault(b => b.Id == id);
            var Recentblogs = db.Blog.OrderByDescending(b => b.Id).Take(3).ToList();
            if (blog == null)
            {
                return NotFound();
            }
            var viewModel = new SingleBlogViewModel
            {
                Blog = blog,
                RecentBlogs = Recentblogs,
                CompanyDetails = companyDetails
            };
            return View(viewModel);
        }
       
    }
}
