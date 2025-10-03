using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    //public class TourCardsViewComponent(MyDbContext db) : ViewComponent
    //{

    //    public IViewComponentResult Invoke(int take = 3)
    //    {
    //        // fetch packages (top 3 for homepage, or all for other pages)
    //        var tours = db.TourPackages.Take(take).ToList();
    //        return View(tours);
    //    }
    //}

    public class TourCardsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
