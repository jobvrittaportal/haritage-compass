using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Admin.Controllers
{
  public class SitePageController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
