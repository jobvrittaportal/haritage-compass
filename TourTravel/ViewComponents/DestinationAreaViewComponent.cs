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
    using System.Drawing.Printing;

    namespace TourTravel.ViewComponents
    {
        public class DestinationAreaViewComponent : ViewComponent
        {
            private readonly MyDbContext _db;
            private readonly IHttpClientFactory _clientFactory;
            private readonly ApiUrlOptions _apiOptions;
            private readonly string _baseUrl;

            public DestinationAreaViewComponent(MyDbContext db, IHttpClientFactory clientFactory, ApiUrlOptions apiOptions)
            {
                _db = db;
                _clientFactory = clientFactory;
                _apiOptions = apiOptions;
                _baseUrl = apiOptions.Use switch
                {
                    "Live" => apiOptions.Live,
                    "Stage" => apiOptions.Stage,
                    "Local" => apiOptions.Local,
                    _ => apiOptions.Live
                };

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
            public async Task<IViewComponentResult> InvokeAsync(bool showheading, int take, string? search, int currentPage = 1)
            {
                List<DestinationViewModel> destinations = new();
                int pageSize = take;
                try
                {
                    var client = _clientFactory.CreateClient();
                    client.BaseAddress = new Uri($"{_baseUrl}/api/");

                    var response = await client.GetFromJsonAsync<List<CityDto>>("Destination");

                    if (response != null)
                    {
                        // Convert DTOs to ViewModels
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
                    Console.WriteLine($"Error fetching destinations: {ex.Message}");
                }

                // Calculate pagination
                int totalItems = destinations.Count;
                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Get only the data for the current page
                var paginatedDestinations = destinations
                    .Skip((currentPage - 1) * pageSize)
                    .Take(take)
                    .ToList();

                ViewBag.ShowHeading = showheading;
                ViewBag.Currentpage = currentPage;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                return View(paginatedDestinations);
            }

        }
    }

}
