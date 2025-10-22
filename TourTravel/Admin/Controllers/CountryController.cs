using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    .Where(x => x.IsDestination == true)
                    .Select(x => new { id = x.Id, name = x.Name })
                    .ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var query = _db.Country.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Code.Contains(search) || c.DialCode.Contains(search));
            }

            int totalItems = await query.CountAsync();
            var list = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/Country/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Country/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country model)
        {
            if (ModelState.IsValid)
            {
                _db.Country.Add(model);
                await _db.SaveChangesAsync();

                TempData["success"] = "Country added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/Country/Create.cshtml", model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var country = await _db.Country.FindAsync(id);
            if (country == null) return NotFound();

            return View("~/Views/Admin/Country/Edit.cshtml", country);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country model)
        {
            var existing = await _db.Country.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.Name = model.Name;
                existing.IsDestination = model.IsDestination;
                existing.Code = model.Code;
                existing.DialCode = model.DialCode;

                await _db.SaveChangesAsync();

                TempData["success"] = "Country updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/Country/Edit.cshtml", model);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _db.Country.FindAsync(id);
            if (country == null)
                return Json(new { success = false, message = "Country not found" });

            _db.Country.Remove(country);
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
