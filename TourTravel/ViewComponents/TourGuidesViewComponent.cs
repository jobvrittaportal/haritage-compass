using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class TourGuidesViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(bool showHeading, int take )
        {
            var TourGuides = db.TourGuideView.OrderByDescending(t => t.Id).Take(take).ToList();
            foreach(var abc in TourGuides)
            {
                if (!string.IsNullOrEmpty(abc.ImageUrl))
                {

                }
            }

            var  viewModel = new TourGuideViewModel
            {
               TourGuides = TourGuides,
                ShowHeading = showHeading // or false depending on logic
            };
            return View(viewModel);
        }
    }
}
