using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Admin.Controllers
{
  public class DashboardController : Controller
  {
    [Authorize]
    [Route("admin/Dashboard")]
    public IActionResult Index()
    {
      return View("Views/admin/Dashboard/Index.cshtml");
    }
  }
}
