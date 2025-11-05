using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TourTravel.Controllers
{

    public class SearchTourController : Controller
    {
      private readonly HttpClient _httpClient;

      public SearchTourController(IHttpClientFactory httpClientFactory)
      {
        _httpClient = httpClientFactory.CreateClient();
      }

      public async Task<IActionResult> Index(int cityId, string checkInDate, string checkOutDate)
      {
        var apiUrl = $"https://localhost:7154/api/package/getPackageList?cityId={cityId}&checkInDate={checkInDate}&checkOutDate={checkOutDate}";

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
    }

  public class PackageDto
  {
    public int Id { get; set; }
    public string Name { get; set; }          // matches "name"
    public string CityName { get; set; }          // matches "name"
    public int Duration { get; set; }         // matches "duration"
    public int CityId { get; set; }           // matches "cityId"
    public string PackageImage { get; set; }  // matches "packageImage"
    public decimal BasePrice { get; set; }    // matches "basePrice"
  }


}
