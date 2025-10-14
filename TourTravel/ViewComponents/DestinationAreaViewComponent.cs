using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
    public class DestinationAreaViewComponent(MyDbContext db) : ViewComponent
    {
        public IViewComponentResult Invoke(bool showHeading)
        {
            var destinations = db.Destinations.ToList();

            ViewData["ShowHeading"] = showHeading;

            return View(destinations);
        }
    }
}
