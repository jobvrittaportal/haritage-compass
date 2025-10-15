using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TourTravel.Admin.Controllers
{
  [Route("admin/location")]
  public class LocationController : Controller
  {
    private readonly MyDbContext _db;
    public LocationController(MyDbContext db)
    {
      _db = db;
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
      var countries = await _db.Country
          .Select(c => new { id = c.Id, name = c.Name })
          .ToListAsync();
      return Json(countries);
    }

    [HttpGet("states/{countryId}")]
    public async Task<IActionResult> GetStates(int countryId)
    {
      var states = await _db.State
          .Where(s => s.CountryId == countryId)
          .Select(s => new { id = s.Id, name = s.Name })
          .ToListAsync();
      return Json(states);
    }

    [HttpGet("cities/{stateId}")]
    public async Task<IActionResult> GetCities(int stateId)
    {
      var cities = await _db.City
          .Where(c => c.StateId == stateId)
          .Select(c => new { id = c.Id, name = c.Name })
          .ToListAsync();
      return Json(cities);
    }
  }

}
