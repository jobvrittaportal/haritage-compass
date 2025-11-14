using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
  public class FeaturedToursViewComponent : ViewComponent
  {
    private readonly IHttpClientFactory _clientFactory;

    public FeaturedToursViewComponent(IHttpClientFactory clientFactory)
    {
      _clientFactory = clientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      var client = _clientFactory.CreateClient();

      var response = await client.GetAsync("https://stg-jungleave-back.jobvritta.com/api/package/getIsFeatureList");

      if (!response.IsSuccessStatusCode)
        return View(new List<FeaturedTourDto>());

      var json = await response.Content.ReadAsStringAsync();

      var featuredTours = JsonSerializer.Deserialize<List<FeaturedTourDto>>(json,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      return View(featuredTours);
    }
  }
}
