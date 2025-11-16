using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
  public class DestinationDropdownViewComponent : ViewComponent
  {
    private readonly IHttpClientFactory _clientFactory;

    public DestinationDropdownViewComponent(IHttpClientFactory clientFactory)
    {
      _clientFactory = clientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      List<DestinationDropdown> destinations = new();

      try
      {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://jungleavengers-api.jobvritta.com/api/");
        var response = await client.GetFromJsonAsync<List<DestinationDropdown>>("Destination/getDestinationSearch");

        if (response != null)
        {
          destinations = response.Select(c => new DestinationDropdown
          {
            Id = c.Id,
            Destination = c.Destination
          }).ToList();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error fetching destinations: {ex.Message}");
      }

      return View(destinations); // Views/Shared/Components/DestinationDropdown/Default.cshtml
    }
  }

  public class DestinationDropdown
  {
    public int Id { get; set; }
    public string Destination { get; set; }
  }
}
