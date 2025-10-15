// StateController.cs
using Microsoft.AspNetCore.Mvc;

[Route("state")]
public class StateController : Controller
{
  private readonly MyDbContext db;
  public StateController(MyDbContext context) { db = context; }

  // GET: state/touristplaces/{countryId}
  [HttpGet("touristplaces/{countryId}")]
  public IActionResult GetTouristPlaces(int countryId)
  {
    try
    {
      var states = db.State
          .Where(s => s.CountryId == countryId && s.IsTouristDestination == true)
          .Select(s => new { s.Id, s.Name })
          .ToList();

      return Ok(states);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { success = false, error = ex.Message });
    }
  }
}
