using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Route("admin/[controller]")]
  public class TestimonialController : Controller
  {
    private readonly MyDbContext _db;
    private readonly IWebHostEnvironment _env;

    public TestimonialController(MyDbContext db, IWebHostEnvironment env)
    {
      _db = db;
      _env = env;
    }

    // ✅ List all images
    //[HttpGet]
    //public IActionResult Index()
    //{
    //  var images = _db.Testimonial.OrderByDescending(x => x.Id).ToList();
    //  return View("~/Views/Admin/Testimonial/Index.cshtml", images);
    //}



    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
    {
      var query = _db.Testimonial.AsQueryable();

      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(b => b.AuthorName.Contains(search) || b.AuthorName.Contains(search));
      }

      int totalItems = await query.CountAsync();
      var blogs = await query
          .OrderByDescending(b => b.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/Testimonial/Index.cshtml", blogs);
    }


    // ✅ Create Page
    [HttpGet("create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/Testimonial/Create.cshtml");
    }

    // ✅ Upload Image
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Testimonial model, IFormFile? image)
    {

      var existingAuthor = _db.Testimonial
        .FirstOrDefault(t => t.AuthorName.Trim().ToLower() == model.AuthorName.Trim().ToLower());

      if (existingAuthor != null)
      {
        return Json(new { success = false, message = "This author already exists!" });
      }

      if (image != null && image.Length > 0)
      {
        string folderPath = Path.Combine(_env.WebRootPath, "Testimonial");
        Directory.CreateDirectory(folderPath);

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        string filePath = Path.Combine(folderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          image.CopyTo(stream);
        }

        model.ImageUrl = "/Testimonial/" + uniqueFileName;

        _db.Testimonial.Add(model);
        _db.SaveChanges();
        return Json(new { success = true, message = "  Added successfully!" });
      }
      else
      {
        TempData["Error"] = "Please select an image to upload.";
        return View("~/Views/Admin/Testimonial/Index.cshtml", model);
      }

     // return RedirectToAction("Index");
    }

    // ✅ Edit Page
    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
      var image = _db.Testimonial.Find(id);
      if (image == null) return NotFound();
      return View("~/Views/Admin/Testimonial/Edit.cshtml", image);
    }

    // ✅ Update Image
    [HttpPost("edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit( Testimonial model, IFormFile? image)
    {
      var existing = _db.Testimonial.Find(model.Id);
      if (existing == null) return NotFound();

      var duplicateAuthor = _db.Testimonial
          .FirstOrDefault(t => t.AuthorName.Trim().ToLower() == model.AuthorName.Trim().ToLower() && t.Id != model.Id);

      if (duplicateAuthor != null)
      {
        return Json(new { success = false, message = "This author already exists!" });
      }

      existing.AuthorName = model.AuthorName;
      existing.Role = model.Role;
      existing.Quote = model.Quote;
      existing.Rating = model.Rating;
   

      if (image != null && image.Length > 0)
      {
        string folderPath = Path.Combine(_env.WebRootPath, "Testimonial");
        Directory.CreateDirectory(folderPath);

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        string filePath = Path.Combine(folderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          image.CopyTo(stream);
        }

        // Delete old image
        if (!string.IsNullOrEmpty(existing.ImageUrl))
        {
          string oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
          if (System.IO.File.Exists(oldPath))
            System.IO.File.Delete(oldPath);
        }

        existing.ImageUrl = "/Testimonial/" + uniqueFileName;
      }

      _db.SaveChanges();
      return Json(new { success = true, message = "  Updated successfully!" });
    //  return RedirectToAction("Index");
    }

    // ✅ Delete
    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var image = _db.Testimonial.Find(id);
      if (image != null)
      {
        // Delete file from wwwroot
        if (!string.IsNullOrEmpty(image.ImageUrl))
        {
          string filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
          if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
        }

        _db.Testimonial.Remove(image);
        _db.SaveChanges();
        TempData["Success"] = " deleted successfully.";
      }

      return RedirectToAction("Index");
    }
  }
}
