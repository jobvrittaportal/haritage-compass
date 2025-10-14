using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class TourCardsViewComponent(MyDbContext db) : ViewComponent
    {

        public IViewComponentResult Invoke(bool ShowHeading, int take, string columnClass = "col-md-6 col-lg-4")
        {

            var tours = db.TourCardsView.Take(take).ToList();
            ViewData["ShowHeading"] = ShowHeading;
            ViewData["ColumnClass"] = columnClass;

            return View(tours);
        }
    }

}
