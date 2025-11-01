using Microsoft.AspNetCore.Mvc;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class TeamViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(int take, int currentPage = 1)
        {
            
            int pageSize = take;
            var query = db.Team.AsQueryable();
            int totalItems = query.Count();
            var Team = query.OrderByDescending(b => b.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Currentpage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(Team);
        }
    }
}
