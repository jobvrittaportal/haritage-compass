using Microsoft.AspNetCore.Mvc;
using TourTravel.Data;

namespace TourTravel.ViewComponents
{
    //public class TourGuidesViewComponent(MyDbContext db) : ViewComponent
    //{
    //    public IViewComponentResult Invoke(int take = 4)
    //    {
    //        var guides = db.TourGuide.Take(take).ToList();
    //        return View(guides);
    //    }
    //}
    public class TourGuidesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
