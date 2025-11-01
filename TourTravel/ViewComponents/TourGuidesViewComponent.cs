using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;
namespace TourTravel.ViewComponents

{

    public class TourGuidesViewComponent(MyDbContext db) : ViewComponent

    {

        public IViewComponentResult Invoke(bool showHeading, int take, int currentPage = 1)

        {

            int pageSize = take;
            var query = db.TourGuideView.AsQueryable();
            int totalItems = query.Count();
            var tourguide = query.OrderByDescending(b => b.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Currentpage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var viewModel = new TourGuideViewModel
            {
              TourGuides = tourguide,
               ShowHeading = showHeading
            };
            return View(viewModel);

        }

    }

}

