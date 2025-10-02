using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using System.Collections.Generic;

namespace TourTravel.ViewComponents
{
    public class CarouselViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Example static data (replace with DB fetch later)
            //var model = new List<CarouselView>
            //{
            //    new CarouselView { BgImg = "/images/banner2.jpg" },
            //    new CarouselView { BgImg = "/images/banner1.jpg" },
            //    new CarouselView { BgImg = "/images/banner3.jpg" }
            //};

            //return View(model); // this will look for Views/Shared/Components/Carousel/Default.cshtml
            return View();
        }
    }
}
