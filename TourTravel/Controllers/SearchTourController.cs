using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TourTravel.Models;

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
          var packages = JsonConvert.DeserializeObject<List<PackageDetailsDto>>(json);
          package = packages?.FirstOrDefault(); // ✅ Take the first one
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
    public int Duration { get; set; }        
    public int CityId { get; set; }         
    public string PackageImage { get; set; } 
    public string? desc { get; set; }  
    public decimal BasePrice { get; set; }
    public int? MaxPerson { get; set; }

  }


}
