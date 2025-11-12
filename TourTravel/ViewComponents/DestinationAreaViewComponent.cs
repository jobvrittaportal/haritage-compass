using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
  using global::TourTravel.Controllers;
  //public class DestinationAreaViewComponent(MyDbContext db) : ViewComponent
  //{
  //    public IViewComponentResult Invoke(bool showHeading, int take)
  //    {
  //        var destinations = db.Destinations.OrderByDescending(t => t.Id).Take(take).ToList();

  //        ViewData["ShowHeading"] = showHeading;

  //        return View(destinations);
  //    }
  //}

  using Microsoft.AspNetCore.Mvc;

  namespace TourTravel.ViewComponents
  {
    public class DestinationAreaViewComponent : ViewComponent
    {
      private readonly MyDbContext _db;
      private readonly IHttpClientFactory _clientFactory;


      public DestinationAreaViewComponent(MyDbContext db, IHttpClientFactory clientFactory)
      {
        _db = db;
        _clientFactory = clientFactory;

      }

      //public IViewComponentResult Invoke(bool showHeading, int take)
      //{
      //  // Fetch top destinations (you can apply any filters like IsTouristDestination == true if needed)
      //  var destinations = _db.Destinations
      //      .OrderByDescending(t => t.Id)
      //      .Take(take)
      //      .Select(c => new DestinationViewModel
      //      {
      //        DestinationName = c.DestinationName,
      //        DestinationImgUrl = c.DestinationImgUrl ?? "/images/default.jpg",
      //        StartingPrice = c.StartingPrice,

      //      })
      //      .ToList();

      //  ViewData["ShowHeading"] = showHeading;

      //  return View(destinations);
      //}
      public async Task<IViewComponentResult> InvokeAsync(bool showHeading)
      {
        List<DestinationViewModel> destinations = new();

        try
        {
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
              StartingPrice = c.BasePrice
            }).ToList();
          }
        }
        catch (Exception ex)
        {
          // You can log or display an error message here
          Console.WriteLine($"Error fetching destinations: {ex.Message}");
        }
        ViewData["ShowHeading"] = showHeading;

        return View(destinations);
      }
    }
  }

}
