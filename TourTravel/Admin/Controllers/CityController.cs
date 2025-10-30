using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Authorize]
  [Route("admin/city")]
  public class CityController : Controller
  {
    private readonly MyDbContext _db;

    public CityController(MyDbContext db)
    {
      _db = db;
    }

    [HttpGet]
    public IActionResult Index(string? search, int page = 1, int pageSize = 15)
    {
      var query = _db.City
          .Include(c => c.State)
          .ThenInclude(s => s.Country)
          .AsQueryable();

      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(c =>
            c.Name.Contains(search) ||
            c.State.Name.Contains(search) ||
            c.State.Country.Name.Contains(search));
      }

      int totalItems = query.Count();

      var cities = query
          .OrderByDescending(c => c.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(c => new CityListDto
          {
            Id = c.Id,
            Name = c.Name,
            StateName = c.State.Name,
            CountryName = c.State.Country.Name,
            IsTouristDestination = c.IsTouristDestination
          })
          .ToList();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/City/Index.cshtml", cities);
    }

    // ✅ Add/Edit/Delete logic for City only (if you need)

    public class CityListDto
    {
      public int Id { get; set; }
      public string Name { get; set; } = string.Empty;
      public string StateName { get; set; } = string.Empty;
      public string CountryName { get; set; } = string.Empty;
      public bool IsTouristDestination { get; set; }
    }
  }
}
