// CityController.cs
using Microsoft.AspNetCore.Mvc;

[Route("city")]
public class CityController : Controller
{
  private readonly MyDbContext db;
  public CityController(MyDbContext context) { db = context; }

  // GET: city/touristplaces/{stateId}
  [HttpGet("touristplaces/{stateId}")]
  public IActionResult GetTouristPlaces(int stateId)
  {
    try
    {
      var cities = db.City
          .Where(s => s.StateId == stateId && s.IsTouristDestination == true)
          .Select(s => new { s.Id, s.Name })
          .ToList();

      return Ok(cities);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { success = false, error = ex.Message });
    }
  }
}
