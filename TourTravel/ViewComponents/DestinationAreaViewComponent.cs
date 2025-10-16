using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
    public class DestinationAreaViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(bool showHeading, int take)
        {
            var destinations = db.Destinations.OrderByDescending(t => t.Id).Take(take).ToList();

            ViewData["ShowHeading"] = showHeading;

            return View(destinations);
        }
    }
}
