using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TourTravel.ViewComponents
{
    //public class BlogViewComponent(MyDbContext db) : ViewComponent
    //{
    //    public IViewComponentResult Invoke(bool showHeading = true)
    //    {
    //        var blogs = db.Blogs
    //                            .OrderByDescending(b => b.CreatedAt)
    //                            .Take(3)
    //                            .ToList();

    //        ViewData["ShowHeading"] = showHeading;

    //        return View(blogs);
    //    }
    //}

    public class BlogViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(bool showHeading )
        {

            ViewData["ShowHeading"] = showHeading;

            return View();
        }
    }
}
