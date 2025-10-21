using Microsoft.AspNetCore.Mvc;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class TeamViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(int take)
        {
            var TourGuides = db.TourGuideView.OrderByDescending(t => t.Id).Take(take).ToList();

            return View(TourGuides);
        }
    }
}
