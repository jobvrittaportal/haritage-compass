using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
   
    public class BlogViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(bool  showheading, int take)
        {
            var blog = db.Blog.OrderByDescending(b => b.Id).Take(take).ToList();
            var viewModel = new BlogViewModel
            {
                Blogs = blog,
                ShowHeading = showheading
            };
            return View(viewModel);
        }
    }
}
