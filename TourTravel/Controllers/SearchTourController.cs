using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{

    public class SearchTourController : Controller
    {
      private readonly HttpClient _httpClient;

      public SearchTourController(IHttpClientFactory httpClientFactory)
      {
        _httpClient = httpClientFactory.CreateClient();
      }

      public async Task<IActionResult> Index(int cityId, string checkInDate, string checkOutDate, int maxPerson)
      {
        var apiUrl = $"https://localhost:7154/api/package/getPackageList?cityId={cityId}&checkInDate={checkInDate}&checkOutDate={checkOutDate}&maxPerson={maxPerson}";

        List<PackageDto> packages = new List<PackageDto>();

        try
        {
          var response = await _httpClient.GetAsync(apiUrl);
          if (response.IsSuccessStatusCode)
          {
            var json = await response.Content.ReadAsStringAsync();
            packages = JsonConvert.DeserializeObject<List<PackageDto>>(json);
          }
        }
        catch
        {
          // Log the error
        }

        return View(packages);
      }

    public async Task<IActionResult> SearchTourDetails(int id)
    {
      var apiUrl = $"https://localhost:7154/api/package/getPackageDetails?id={id}";
      PackageDetailsDto package = null;

      try
      {
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

  public class PackageDto
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
  public class PackageDetailsDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CityName { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public int Duration { get; set; }
    public int CityId { get; set; }
    public string PackageImage { get; set; }
    public string Desc { get; set; }
    public decimal BasePrice { get; set; }
    public int? MaxPerson { get; set; }

    public List<SpotDto> Spots { get; set; } = new();
  }
  public class SpotDto
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int CityId { get; set; }
    public List<SpotImageDto> Images { get; set; } = new();
  }


  public class SpotImageDto
  {
    public int Id { get; set; }
    public string ImageType { get; set; }
    public bool? IsCoverPhoto { get; set; }
    public int? OrderIndex { get; set; }
    public string ImageUrl { get; set; }
  }



}
