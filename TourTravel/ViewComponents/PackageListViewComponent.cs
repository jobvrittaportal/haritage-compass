using Microsoft.AspNetCore.Mvc;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class PackageListViewComponent : ViewComponent
    {
    private readonly IHttpClientFactory _clientFactory;

    public PackageListViewComponent(IHttpClientFactory clientFactory)
    {
      _clientFactory = clientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
      var client = _clientFactory.CreateClient();
      var apiUrl = $"https://jungleavengers-api.jobvritta.com/api/Destination/getAllSpots?id={id}";

      List<SpotViewModel>? allSpots = new();

      try
      {
        allSpots = await client.GetFromJsonAsync<List<SpotViewModel>>(apiUrl);
      }
      catch (Exception ex)
      {
      }

      return View(allSpots ?? new List<SpotViewModel>());
    }
  }
    
    
}
