using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TourTravel.Controllers
{
  public class ToursController : Controller
  {
    private readonly HttpClient _httpClient;
    private readonly MyDbContext db;

    public ToursController(IHttpClientFactory httpClientFactory, MyDbContext db)
    {
      this.db = db;
      _httpClient = httpClientFactory.CreateClient();
    }
    //public IActionResult Index()
    //{

    //  var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package");
    //  if (page != null)
    //  {
    //    ViewBag.Title = page.Title;
    //    ViewBag.Page = page.Page;
    //    ViewBag.Description = page.Description;
    //    ViewBag.Keywords = page.KeyWords;
    //    ViewBag.Image = page.Image;
    //    ViewBag.ImageHeight = page.ImgHeight;
    //    ViewBag.ImageWidth = page.ImgWidth;
    //  }

    //  return View();
    //}


    public async Task<IActionResult> Index()
    {
      var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Page = page.Page;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }
      var apiUrl = $"https://stg-jungleave-back.jobvritta.com/api/package/getTourPackage";
      List<PackagessDto> packages = new List<PackagessDto>();
      try
      {
        var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
          var json = await response.Content.ReadAsStringAsync();
          packages = JsonConvert.DeserializeObject<List<PackagessDto>>(json);
        }
      }
      catch
      {
      }
      return View(packages);
    }

    //[HttpGet]
    //public async Task<IActionResult> GetFilteredPackages(
    //  int? DestinationId,
    //  string? checkInDate,
    //  string? checkOutDate,
    //  decimal? minPrice,
    //  decimal? maxPrice,
    //  int? minDuration,
    //  int? maxDuration)
    //{
    //  try
    //  {
    //    return ViewComponent("TourCards", new
    //    {
    //      DestinationId,
    //      checkInDate,
    //      checkOutDate,
    //      minPrice,
    //      maxPrice,
    //      minDuration,
    //      maxDuration,
    //      ShowHeading = false,
    //      take = 8,
    //      columnClass = "col-md-6 col-lg-6"
    //    });
    //  }
    //  catch (Exception ex)
    //  {
    //    return Content($"<p>Error loading packages: {ex.Message}</p>", "text/html");
    //  }
    //}


    [HttpGet]
    public async Task<IActionResult> GetFilteredPackages(
      int? DestinationId,
      string? checkInDate,
      string? checkOutDate,
      decimal? minPrice,
      decimal? maxPrice,
      int? minDuration,
      int? maxDuration)
    {
      try
      {
        string html = await this.RenderViewComponentToStringAsync("TourCards", new
        {
          DestinationId,
          checkInDate,
          checkOutDate,
          minPrice,
          maxPrice,
          minDuration,
          maxDuration,
          ShowHeading = false,
          take = 8,
          columnClass = "col-md-6 col-lg-6"
        });

        return Content(html, "text/html");
      }
      catch (Exception ex)
      {
        return Content($"<p>Error loading packages: {ex.Message}</p>", "text/html");
      }
    }








    public IActionResult TourPackageOffer()
    {
      var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package Offer");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Page = page.Page;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }
      return View();
    }
    public IActionResult TourPackageCart()
    {

      var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Cart");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Page = page.Page;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }
      return View();
    }
    public IActionResult TourPackageBookings()
    {

      var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Package Booking");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Page = page.Page;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }
      return View();
    }
    public IActionResult TourPackageConfirm()
    {

      var page = db.SitePages.FirstOrDefault(f => f.Page == "Booking Confirm");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Page = page.Page;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }
      return View();
    }
    public IActionResult TourPackageSearch()
    {

      var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Search");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Page = page.Page;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }
      return View();
    }
    //public IActionResult TourPackageDetails(int id)
    //{

    //  var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Details");
    //  if (page != null)
    //  {
    //    ViewBag.Title = page.Title;
    //    ViewBag.Page = page.Page;
    //    ViewBag.Description = page.Description;
    //    ViewBag.Keywords = page.KeyWords;
    //    ViewBag.Image = page.Image;
    //    ViewBag.ImageHeight = page.ImgHeight;
    //    ViewBag.ImageWidth = page.ImgWidth;
    //  }
    //  var packagedetail = db.TourCardsView.FirstOrDefault(x => x.Id == id);
    //  return View(packagedetail);
    //}

    public async Task<IActionResult> TourPackageDetails(int id)
    {
      var apiUrl = $"https://stg-jungleave-back.jobvritta.com/api/package/getPackageDetails?id={id}";
      PackageDetailsDto package = null;

      try
      {
                var page = db.SitePages.FirstOrDefault(f => f.Page == "Tour Details");
                if (page != null)
                     {
                       ViewBag.Title = page.Title;
                       ViewBag.Page = page.Page;
                       ViewBag.Description = page.Description;
                       ViewBag.Keywords = page.KeyWords;
                       ViewBag.Image = page.Image;
                       ViewBag.ImageHeight = page.ImgHeight;
                       ViewBag.ImageWidth = page.ImgWidth;
                     }
                    var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
          var json = await response.Content.ReadAsStringAsync();
          package = JsonConvert.DeserializeObject<PackageDetailsDto>(json);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return View(package);
    }
  }

  public class PackagessDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CityName { get; set; }
    public int Duration { get; set; }
    public int CityId { get; set; }
    public string PackageImage { get; set; }
    public decimal BasePrice { get; set; }
    public int? MaxPerson { get; set; }

  }
  
}
