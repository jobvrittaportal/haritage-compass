using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
  public class DestinationsController : Controller
  {
    private readonly MyDbContext db;
    private readonly IHttpClientFactory _clientFactory;

    public DestinationsController(IHttpClientFactory clientFactory, MyDbContext db)
    {
      this.db = db;

      _clientFactory = clientFactory;
    }
    //public IActionResult Index()
    //{
    //  ViewBag.Page = "Destinations";
    //  var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
    //  if (page != null)
    //  {
    //    ViewBag.Title = page.Title;
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
      List<DestinationViewModel> destinations = new();

      try
      {
        ViewBag.Page = "Destinations";
        var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
        if (page != null)
        {
          ViewBag.Title = page.Title;
          ViewBag.Description = page.Description;
          ViewBag.Keywords = page.KeyWords;
          ViewBag.Image = page.Image;
          ViewBag.ImageHeight = page.ImgHeight;
          ViewBag.ImageWidth = page.ImgWidth;
        }
        // Create HTTP client
        var client = _clientFactory.CreateClient();

        // Set base address of your API
        client.BaseAddress = new Uri("https://stg-jungleave-back.jobvritta.com/api/");

        // Call the API that returns an array of city objects
        var response = await client.GetFromJsonAsync<List<CityDto>>("Destination");

        if (response != null)
        {
          destinations = response.Select(c => new DestinationViewModel
          {
            Id = c.Id,
            DestinationName = c.Name,
            DestinationImgUrl = c.ImageUrl ?? "/img/bg/cta.jpg",
            StartingPrice = c.BasePrice,
          }).ToList();
        }
      }
      catch (Exception ex)
      {
        // You can log or display an error message here
        Console.WriteLine($"Error fetching destinations: {ex.Message}");
      }

      return View(destinations);
    }




    //public IActionResult DestinationDetails(int Id)
    //{
    //  ViewBag.Page = "Destination Detail";
    //  var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
    //  if (page != null)
    //  {
    //    ViewBag.Title = page.Title;
    //    ViewBag.Description = page.Description;
    //    ViewBag.Keywords = page.KeyWords;
    //    ViewBag.Image = page.Image;
    //    ViewBag.ImageHeight = page.ImgHeight;
    //    ViewBag.ImageWidth = page.ImgWidth;
    //  }

    //  var destination = db.Destinations.FirstOrDefault(d => d.Id == Id);
    //  if (destination == null) return NotFound();

    //  var galleryImages = db.DestinationGallery.Where(g => g.DestinationId == Id).ToList();

    //  var viewModel = new TourTravel.ViewModel.DestinationDetailsViewModel
    //  {
    //    Destination = destination,
    //    GalleryImages = galleryImages
    //  };

    //  return View(viewModel);
    //}


    //[HttpGet]
    //public async Task<IActionResult> DestinationDetails(int id)
    //{
    //  ViewBag.Page = "Destination Detail";

    //  // Meta tags
    //  var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
    //  if (page != null)
    //  {
    //    ViewBag.Title = page.Title;
    //    ViewBag.Description = page.Description;
    //    ViewBag.Keywords = page.KeyWords;
    //    ViewBag.Image = page.Image;
    //    ViewBag.ImageHeight = page.ImgHeight;
    //    ViewBag.ImageWidth = page.ImgWidth;
    //  }

    //  var viewModel = new DestinationDetailsViewModel();

    //  try
    //  {
    //    var client = _clientFactory.CreateClient();
    //    client.BaseAddress = new Uri("https://localhost:7154/api/");

    //    // ✅ Fetch all spots for the destination
    //    var response = await client.GetFromJsonAsync<List<SpotDetailDto>>($"Destination/getDestDetails?id={id}");

    //    if (response != null && response.Any())
    //    {
    //      // Try to get city name from your DB (optional)
    //      var city = db.City.FirstOrDefault(c => c.Id == id);
    //      viewModel.CityName = city?.Name ?? "Destination";

    //      // Cover image from API (if one marked as cover)
    //      viewModel.CityCoverImage = response.FirstOrDefault(r => r.IsCoverPhoto)?.ImageUrl;

    //      // Sort by orderIndex if available
    //      viewModel.Spots = response.OrderBy(r => r.OrderIndex ?? 0).ToList();

    //      // ✅ Dummy map for Varanasi (or dynamic later)
    //      if (viewModel.CityName.ToLower().Contains("varanasi"))
    //      {
    //        viewModel.MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3560.440037562953!2d83.00572847515765!3d25.31764572923105!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x398e2e2ed097ccff%3A0x4c4d893f7b7d21f8!2sVaranasi%2C%20Uttar%20Pradesh!5e0!3m2!1sen!2sin!4v1698664099872!5m2!1sen!2sin%22;
    //      }
    //      else
    //      {
    //        // Generic India map if no specific city found
    //        viewModel.MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3109311.309053843!2d74.88509104774865!3d20.59368435709005!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zMjDCsDM1JzM3LjMiTiA3NMKwNTMnMjMuMiJF!5e0!3m2!1sen!2sin!4v1698664237564!5m2!1sen!2sin%22;
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Console.WriteLine($"Error fetching destination details: {ex.Message}");
    //  }

    //  return View(viewModel);
    //}
    [HttpGet]
    public async Task<IActionResult> DestinationDetails(int id)
    {
      ViewBag.Page = "Destination Detail";

      var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
      if (page != null)
      {
        ViewBag.Title = page.Title;
        ViewBag.Description = page.Description;
        ViewBag.Keywords = page.KeyWords;
        ViewBag.Image = page.Image;
        ViewBag.ImageHeight = page.ImgHeight;
        ViewBag.ImageWidth = page.ImgWidth;
      }

      var viewModel = new DestinationDetailsViewModel();

      try
      {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://stg-jungleave-back.jobvritta.com/api/");

        var response = await client.GetFromJsonAsync<DestinationDetailsViewModel>(
            $"Destination/getDestinationDetails?id={id}");

        if (response != null)
        {
          viewModel.Id = response.Id;
          viewModel.CityName = response.CityName ?? response.Name ?? "Explore Destination";
          viewModel.CityDescription = response.CityDescription ?? response.Description;
          viewModel.CityCoverImage = response.CityCoverImage ?? response.CoverPhoto;
          viewModel.Gallery = response.Gallery ?? new List<ImageDto>();
          viewModel.MapUrl = "https://www.google.com/maps/embed?pb=...";
          viewModel.Latitude = response.Latitude ?? 0;
          viewModel.Longitude = response.Longitude ?? 0;

        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error fetching destination details: {ex.Message}");
      }

      return View(viewModel);
    }


    public async Task<IActionResult> SpotDetails(int id)
    {
      ViewBag.Page = "Spot Detail";

      var client = _clientFactory.CreateClient();
      var apiUrl = $"https://stg-jungleave-back.jobvritta.com/api/Destination/getSpotsDetails?id={id}";

      List<SpotDetailsViewModel>? spots = null;

      try
      {
        spots = await client.GetFromJsonAsync<List<SpotDetailsViewModel>>(apiUrl);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error fetching spot details: {ex.Message}");
      }

      var spot = spots?.FirstOrDefault();

      if (spot == null)
        return NotFound();

      return View(spot);
    }



  }


  public class CityDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsTouristDestination { get; set; }
    public bool IsDisplayHome { get; set; }
    public string ImageFileName { get; set; }
    public string ImageUrl { get; set; }
    public bool IsCoverPhoto { get; set; }
    public int? OrderIndex { get; set; }
    public decimal? BasePrice { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
  }



}
