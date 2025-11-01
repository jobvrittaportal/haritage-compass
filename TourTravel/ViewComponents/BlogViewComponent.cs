using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
   
    public class BlogViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(bool  showheading, int take,string ? search, int currentPage = 1)
        {
            
            int pageSize = take;
            var query = db.Blog.AsQueryable();

            // 🔍 Search filter by Title or Author

            if (!string.IsNullOrEmpty(search))
            {
              query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            int totalItems = query.Count();
            var blog = query
                .OrderByDescending(b => b.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.Search = search;
            ViewBag.Currentpage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);          
            var viewModel = new BlogViewModel
            {
                Blogs = blog,
                ShowHeading = showheading
            };
            return View(viewModel);
        }
    }
}
