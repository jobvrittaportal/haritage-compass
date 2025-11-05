using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
  public class TourTypeDropdownViewComponent : ViewComponent
  {
    private readonly IHttpClientFactory _clientFactory;

    public TourTypeDropdownViewComponent(IHttpClientFactory clientFactory)
    {
      _clientFactory = clientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      List<TourTypeDropdown> tourType = new();

      try
      {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7154/api/");
        var response = await client.GetFromJsonAsync<List<TourTypeDropdown>>("TourType/dropdown");

        if (response != null)
        {
          tourType = response.Select(c => new TourTypeDropdown
          {
            Id = c.Id,
            Name = c.Name
          }).ToList();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error fetching destinations: {ex.Message}");
      }

      return View(tourType); // Views/Shared/Components/DestinationDropdown/Default.cshtml
    }
  }

  public class TourTypeDropdown
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
