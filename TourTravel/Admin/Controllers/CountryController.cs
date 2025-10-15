using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Route("admin/country")]
  public class CountryController : Controller
  {
    private readonly MyDbContext _db;

    public CountryController(MyDbContext db)
    {
      _db = db;
    }

    // GET: /admin/country/touristplaces
    [HttpGet("touristplaces")]
    public IActionResult GetTouristPlaces()
    {
      try
      {
        var data = _db.Country
            .Where(x => x.IsTouristDestination == true)
            .Select(x => new { id = x.Id, name = x.Name })
            .ToList();

        return Json(data);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { success = false, message = ex.Message });
      }
    }
  }
}
