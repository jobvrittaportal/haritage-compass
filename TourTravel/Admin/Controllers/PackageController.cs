using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Authorize]
  [Route("admin/Package")]
  public class PackageController : Controller
  {
    private readonly MyDbContext _db;
    private readonly IWebHostEnvironment _env;

    public PackageController(MyDbContext db, IWebHostEnvironment env)
    {
      _db = db;
      _env = env;
    }

    // ✅ Package List + Search + Pagination
    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
    {
      var query = _db.Package.AsQueryable();

      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(b => b.Name.Contains(search) );
      }

      int totalItems = await query.CountAsync();
      var package = await query
          .OrderByDescending(b => b.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/Package/Index.cshtml", package);
    }

    // ✅ Render Create Form
    [HttpGet("Create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/Package/Create.cshtml");
    }

    // ✅ Create package (AJAX)
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Package model, IFormFile? image)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Desc))
          return Json(new { success = false, message = "Please fill all required fields." });

        if (image == null || image.Length == 0)
          return Json(new { success = false, message = "Please upload a Package image." });

        bool slugExists = await _db.Package.AnyAsync(b => b.SlugUrl.ToLower() == model.SlugUrl.ToLower());
        bool titleExist = await _db.Package.AnyAsync(b => b.Name.ToLower() == model.Name.ToLower());

        if (slugExists)
          return Json(new { success = false, message = "Slug URL already exists." });

        if (titleExist)
          return Json(new { success = false, message = "This package already exists." });


        // ✅ Validate file size (max 5MB)
        if (image.Length > 5 * 1024 * 1024)
          return Json(new { success = false, message = "Image size must be less than 5MB." });

        // ✅ Validate dimensions
        using (var stream = image.OpenReadStream())
        using (var img = Image.FromStream(stream))
        {
          if (img.Width != 800 || img.Height != 600)
            return Json(new { success = false, message = "Image dimensions must be 800x600." });
        }

        string folderPath = Path.Combine(_env.WebRootPath, "Package");
        Directory.CreateDirectory(folderPath);

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        string filePath = Path.Combine(folderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await image.CopyToAsync(stream);
        }

        model.ImageUrl = "/Package/" + uniqueFileName;

        _db.Package.Add(model);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "Package created successfully." });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }

    // ✅ Render Edit Form
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
      var package = await _db.Package.FindAsync(id);
      if (package == null) return NotFound();

      return View("~/Views/Admin/Package/Edit.cshtml", package);
    }

    // ✅ Edit Package (AJAX)
    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Package model, IFormFile? image)
    {
      try
      {
        var existing = await _db.Package.FindAsync(model.Id);
        if (existing == null)
          return Json(new { success = false, message = "Record not found." });

        bool slugExists = await _db.Package
            .AnyAsync(b => b.Id != model.Id && b.SlugUrl.ToLower() == model.SlugUrl.ToLower());



        bool titleExists = await _db.Package
            .AnyAsync(b => b.Id != model.Id && b.Name.ToLower() == model.Name.ToLower());

        if (slugExists)
          return Json(new { success = false, message = "Slug URL already exists." });


        if (titleExists)
          return Json(new { success = false, message = " already exists." });


        // ✅ Validate file size (max 5MB)
        //if (image.Length > 5 * 1024 * 1024)
        //  return Json(new { success = false, message = "Image size must be less than 5MB." });

        //// ✅ Validate dimensions
        //using (var stream = image.OpenReadStream())
        //using (var img = Image.FromStream(stream))
        //{
        //  if (img.Width != 800 || img.Height != 600)
        //    return Json(new { success = false, message = "Image dimensions must be 800x600." });
        //}


        existing.Name = model.Name;
        existing.Duration = model.Duration;
        existing.Desc = model.Desc;
        existing.IsActive = model.IsActive;
        existing.Price = model.Price;
        existing.ReviewsCount = model.ReviewsCount;
        existing.MaxPerson = model.MaxPerson;
        existing.MetaTitle = model.MetaTitle;
        existing.MetaDescription = model.MetaDescription;
        existing.SlugUrl = model.SlugUrl;

        if (image != null && image.Length > 0)
        {
          string folderPath = Path.Combine(_env.WebRootPath, "Package");
          Directory.CreateDirectory(folderPath);

          string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
          string filePath = Path.Combine(folderPath, uniqueFileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await image.CopyToAsync(stream);
          }

          if (!string.IsNullOrEmpty(existing.ImageUrl))
          {
            string oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldPath))
              System.IO.File.Delete(oldPath);
          }

          existing.ImageUrl = "/Package/" + uniqueFileName;
        }

        _db.Update(existing);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "Package updated successfully." });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }

    // ✅ Delete Package
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      var package = await _db.Package.FindAsync(id);
      if (package == null) return NotFound();

      if (!string.IsNullOrEmpty(package.ImageUrl))
      {
        string filePath = Path.Combine(_env.WebRootPath, package.ImageUrl.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
          System.IO.File.Delete(filePath);
      }

      _db.Package.Remove(package);
      await _db.SaveChangesAsync();

      TempData["Success"] = "Package deleted successfully.";
      return RedirectToAction(nameof(Index));
    }
  }
}

