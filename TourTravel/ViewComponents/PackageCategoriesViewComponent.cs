using Microsoft.AspNetCore.Mvc;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class PackageCategoriesViewComponent:ViewComponent
    {
    private readonly IHttpClientFactory _clientFactory;

    // ✅ Constructor to inject HttpClient
    public PackageCategoriesViewComponent(IHttpClientFactory clientFactory)
    {
      _clientFactory = clientFactory;
    }

    // ✅ Async Invoke method to fetch data from API
    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
      var client = _clientFactory.CreateClient();
      var apiUrl = $"https://localhost:7154/api/Destination/getAllSpots?id={id}";

      List<SpotViewModel>? allSpots = new();

      try
      {
        allSpots = await client.GetFromJsonAsync<List<SpotViewModel>>(apiUrl);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"❌ Error fetching spots: {ex.Message}");
      }

      // Return the view with data (Model)
      return View(allSpots ?? new List<SpotViewModel>());
    }
  }
    
    
}
