using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TourTravel.Controllers;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
  public class TourSearchViewComponent : ViewComponent
  {
    private readonly HttpClient _httpClient;
    private readonly MyDbContext _db;

    public TourSearchViewComponent(MyDbContext db)
    {
      _db = db;
      _httpClient = new HttpClient();
    }

    public async Task<IViewComponentResult> InvokeAsync(
        bool ShowHeading = true,
        int take = 6,
        string columnClass = "col-md-6 col-lg-4",
        int? DestinationId = null,
        string? checkInDate = null,
        string? checkOutDate = null,
        int? maxPerson = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minDuration = null,
        int? maxDuration = null)
    {
      ViewData["ShowHeading"] = ShowHeading;
      ViewData["ColumnClass"] = columnClass;

      List<PackagessDto> tours = new();

      try
      {
        string apiUrl;

        //  CASE 1: If DestinationId + Dates exist → use getPackageList
        if (DestinationId.HasValue &&
            !string.IsNullOrEmpty(checkInDate) &&
            !string.IsNullOrEmpty(checkOutDate))
        {
          var baseUrl = "https://stg-jungleave-back.jobvritta.com/api/package/getPackageList?";
          var parameters = new List<string>
                {
                    $"cityId={DestinationId}",
                    $"checkInDate={checkInDate}",
                    $"checkOutDate={checkOutDate}"
                };

          if (maxPerson.HasValue)
            parameters.Add($"MaxPerson={maxPerson}");

          if (minPrice.HasValue)
            parameters.Add($"minPrice={minPrice}");

          if (maxPrice.HasValue)
            parameters.Add($"maxPrice={maxPrice}");

          if (minDuration.HasValue)
            parameters.Add($"minDuration={minDuration}");

          if (maxDuration.HasValue)
            parameters.Add($"maxDuration={maxDuration}");

          apiUrl = baseUrl + string.Join("&", parameters);
        }
        else
        {
          //  CASE 2: Otherwise → use getfilterPackageList (all optional)
          var baseUrl = "https://localhost:7154/api/package/getfilterPackageList?";
          var parameters = new List<string>();

          if (DestinationId.HasValue)
            parameters.Add($"cityId={DestinationId}");

          if (!string.IsNullOrEmpty(checkInDate))
            parameters.Add($"checkInDate={checkInDate}");

          if (!string.IsNullOrEmpty(checkOutDate))
            parameters.Add($"checkOutDate={checkOutDate}");

          if (maxPerson.HasValue)
            parameters.Add($"MaxPerson={maxPerson}");

          if (minPrice.HasValue)
            parameters.Add($"minPrice={minPrice}");

          if (maxPrice.HasValue)
            parameters.Add($"maxPrice={maxPrice}");

          if (minDuration.HasValue)
            parameters.Add($"minDuration={minDuration}");

          if (maxDuration.HasValue)
            parameters.Add($"maxDuration={maxDuration}");

          apiUrl = baseUrl + string.Join("&", parameters);
        }

        //  If no parameters, call default endpoint
        if (string.IsNullOrWhiteSpace(apiUrl))
          apiUrl = "https://stg-jungleave-back.jobvritta.com/api/package/getTourPackage";

        //  Fetch data
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
          var json = await response.Content.ReadAsStringAsync();
          tours = JsonConvert.DeserializeObject<List<PackagessDto>>(json) ?? new List<PackagessDto>();
        }
        else
        {
          Console.WriteLine($"API returned status: {response.StatusCode}");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"[TourCardsViewComponent] Error: {ex.Message}");
      }

      var limitedTours = tours.Take(take).ToList();
      return View(limitedTours);
    }

    //public async Task<IViewComponentResult> InvokeAsync(
    // bool ShowHeading = true,
    // int take = 6,
    // string columnClass = "col-md-6 col-lg-4",
    // int? DestinationId = null,
    // string? checkInDate = null,
    // string? checkOutDate = null,
    // int? maxPerson = null,
    // decimal? minPrice = null,
    // decimal? maxPrice = null,
    // int? minDuration = null,
    // int? maxDuration = null)
    //{
    //  ViewData["ShowHeading"] = ShowHeading;
    //  ViewData["ColumnClass"] = columnClass;

    //  List<PackagessDto> tours = new();

    //  try
    //  {
    //    string apiUrl = "";
    //    bool hasAnyFilter =
    //        DestinationId.HasValue ||
    //        !string.IsNullOrEmpty(checkInDate) ||
    //        !string.IsNullOrEmpty(checkOutDate) ||
    //        maxPerson.HasValue ||
    //        minPrice.HasValue ||
    //        maxPrice.HasValue ||
    //        minDuration.HasValue ||
    //        maxDuration.HasValue;

    //    // -------------------------------------------
    //    // CASE 1: Destination + Dates → getPackageList
    //    // -------------------------------------------
    //    if (DestinationId.HasValue &&
    //        !string.IsNullOrEmpty(checkInDate) &&
    //        !string.IsNullOrEmpty(checkOutDate))
    //    {
    //      var baseUrl = "https://localhost:7154/api/package/getPackageList?";
    //      var parameters = new List<string>
    //        {
    //            $"cityId={DestinationId}",
    //            $"checkInDate={checkInDate}",
    //            $"checkOutDate={checkOutDate}"
    //        };

    //      if (maxPerson.HasValue) parameters.Add($"MaxPerson={maxPerson}");
    //      if (minPrice.HasValue) parameters.Add($"minPrice={minPrice}");
    //      if (maxPrice.HasValue) parameters.Add($"maxPrice={maxPrice}");
    //      if (minDuration.HasValue) parameters.Add($"minDuration={minDuration}");
    //      if (maxDuration.HasValue) parameters.Add($"maxDuration={maxDuration}");

    //      apiUrl = baseUrl + string.Join("&", parameters);
    //    }

    //    // ---------------------------------------------------
    //    // CASE 2: Filters exist (but not full date + dest) →
    //    //          getfilterPackageList
    //    // ---------------------------------------------------
    //    else if (hasAnyFilter)
    //    {
    //      var baseUrl = "https://localhost:7154/api/package/getfilterPackageList?";
    //      var parameters = new List<string>();

    //      if (DestinationId.HasValue) parameters.Add($"cityId={DestinationId}");
    //      if (!string.IsNullOrEmpty(checkInDate)) parameters.Add($"checkInDate={checkInDate}");
    //      if (!string.IsNullOrEmpty(checkOutDate)) parameters.Add($"checkOutDate={checkOutDate}");
    //      if (maxPerson.HasValue) parameters.Add($"MaxPerson={maxPerson}");
    //      if (minPrice.HasValue) parameters.Add($"minPrice={minPrice}");
    //      if (maxPrice.HasValue) parameters.Add($"maxPrice={maxPrice}");
    //      if (minDuration.HasValue) parameters.Add($"minDuration={minDuration}");
    //      if (maxDuration.HasValue) parameters.Add($"maxDuration={maxDuration}");

    //      apiUrl = baseUrl + string.Join("&", parameters);
    //    }

    //    // -----------------------------------------
    //    // CASE 3: No filter at all → TourPackage API
    //    // -----------------------------------------
    //    else
    //    {
    //      apiUrl = "https://localhost:7154/api/package/getTourPackage";
    //    }

    //    // --------------------------
    //    // Execute API Call
    //    // --------------------------
    //    var response = await _httpClient.GetAsync(apiUrl);

    //    if (response.IsSuccessStatusCode)
    //    {
    //      var json = await response.Content.ReadAsStringAsync();
    //      tours = JsonConvert.DeserializeObject<List<PackagessDto>>(json)
    //              ?? new List<PackagessDto>();
    //    }
    //    else
    //    {
    //      Console.WriteLine($"API returned status: {response.StatusCode}");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Console.WriteLine($"[TourCardsViewComponent] Error: {ex.Message}");
    //  }

    //  var limitedTours = tours.Take(take).ToList();
    //  return View(limitedTours);
    //}





  }
}
