using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
  public class BookNowController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
