using Microsoft.AspNetCore.Mvc;

namespace TourTravel.admin.Controllers
{
  public class LoginController : Controller
  {
    [Route("admin/login")]
    public IActionResult Index()
    {
      return View("Views/admin/Login/index.cshtml");
    }

    [Route("login")]

    [HttpPost]
    public IActionResult Index(string username, string password)
    {
      if (username == "admin" && password == "password")
      {
        // Redirect to Dashboard page
        return RedirectToAction("Index", "Dashboard");
      }

      ViewBag.Error = "Invalid credentials!";
      return View("Views/admin/Login/index.cshtml");
    }


  }
}
