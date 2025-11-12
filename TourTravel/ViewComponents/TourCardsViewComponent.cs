using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TourTravel.Controllers; // for PackagessDto
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
  public class TourCardsViewComponent : ViewComponent
  {
    private readonly HttpClient _httpClient;
    private readonly MyDbContext _db;

    public TourCardsViewComponent(MyDbContext db)
    {
      _db = db;
      _httpClient = new HttpClient();
    }

    public async Task<IViewComponentResult> InvokeAsync(bool ShowHeading = true, int take = 6, string columnClass = "col-md-6 col-lg-4")
    {
      ViewData["ShowHeading"] = ShowHeading;
      ViewData["ColumnClass"] = columnClass;

      List<PackagessDto> tours = new();

      try
      {
        string apiUrl = "https://localhost:7154/api/package/getTourPackage";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
          var json = await response.Content.ReadAsStringAsync();
          tours = JsonConvert.DeserializeObject<List<PackagessDto>>(json) ?? new List<PackagessDto>();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"[TourCardsViewComponent] Error: {ex.Message}");
      }

      var limitedTours = tours.Take(take).ToList();

      return View(limitedTours);
    }
  }
}
