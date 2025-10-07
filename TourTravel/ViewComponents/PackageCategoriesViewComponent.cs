using Microsoft.AspNetCore.Mvc;

namespace TourTravel.ViewComponents
{
    public class PackageCategoriesViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
    
    
}
