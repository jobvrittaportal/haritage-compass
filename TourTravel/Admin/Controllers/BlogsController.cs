using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Authorize]
  [Route("admin/blogs")]
  public class BlogsController : Controller
  {
    private readonly MyDbContext _db;
    private readonly IWebHostEnvironment _env;

    public BlogsController(MyDbContext db, IWebHostEnvironment env)
    {
      _db = db;
      _env = env;
    }

    [HttpGet]
    public IActionResult Index(string? search, int page = 1, int pageSize = 5)
    {
      var query = _db.Blog.AsQueryable();

      //  Search filter by Title or Author
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(b =>
            b.Title.Contains(search) || b.Author.Contains(search));
      }

      //  Pagination logic
      int totalItems = query.Count();
      var blogs = query
          .OrderByDescending(b => b.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.PageSize = pageSize;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/Blogs/Index.cshtml", blogs);
    }


    [HttpGet("Create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/Blogs/Create.cshtml");
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Blog model, IFormFile imageUrl)
    {
    
        
          // Generate unique file name
          var fileName = Path.GetFileNameWithoutExtension(imageUrl.FileName);
          var extension = Path.GetExtension(imageUrl.FileName);
          var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

          // Define the upload path (e.g., wwwroot/uploads)
          var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blogs");

          // Ensure directory exists
          if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

          // Full file path
          var filePath = Path.Combine(uploadPath, newFileName);

          // Save file
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await imageUrl.CopyToAsync(stream);
          }

          // Save relative path for display
          model.ImageUrl = "/Blogs/" + newFileName;
        

        _db.Blog.Add(model);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
      

     // return View("~/Views/Admin/Blogs/Index.cshtml",model);
    }


    [HttpGet("Edit/{id}")]
    public IActionResult Edit(int id)
    {
      var blog = _db.Blog.Find(id);
      if (blog == null) return NotFound();

      return View("~/Views/Admin/Blogs/Edit.cshtml", blog);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Blog model, IFormFile imageFile)
    {
      var existing = _db.Blog.Find(id);
      if (existing == null) return NotFound();

      if (ModelState.IsValid)
      {
        existing.Title = model.Title;
        existing.Author = model.Author;
        existing.Content = model.Content;

        if (imageFile != null && imageFile.Length > 0)
        {
          string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/blogs");
          Directory.CreateDirectory(uploadsFolder);

          string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
          string filePath = Path.Combine(uploadsFolder, uniqueFileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            imageFile.CopyTo(stream);
          }

          existing.ImageUrl = "/uploads/blogs/" + uniqueFileName;
        }

        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(model);
    }

    [HttpPost("Delete/{id}")]
    public IActionResult Delete(int id)
    {
      var blog = _db.Blog.Find(id);
      if (blog == null) return NotFound();

      _db.Blog.Remove(blog);
      _db.SaveChanges();

      return RedirectToAction("Index");
    }
  }
}
