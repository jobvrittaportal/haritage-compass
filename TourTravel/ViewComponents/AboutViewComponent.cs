using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
    public class AboutViewComponent(MyDbContext db) : ViewComponent
    {
        private readonly MyDbContext db = db;
        public IViewComponentResult Invoke()
        {
            var about = db.About.FirstOrDefault();
            var viewModel = new About
            {
                Title = about?.Title,
                Description = about?.Description,
                Image = about?.Image,
                KeyFeatures = about?.KeyFeatures,
                MetaTitle = about?.MetaTitle,
                MetaDescription = about?.MetaDescription,
                SlugUrl = about?.SlugUrl
            };
            return View(viewModel);
        }
    }
}
