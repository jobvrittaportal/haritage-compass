using Microsoft.AspNetCore.Mvc;

namespace TourTravel.Controllers
{
  public class TourCardsController : Controller
  {
[HttpGet]
    public IActionResult Filter(
   string columnClass,
        int? DestinationId,
        string? checkInDate,
        string? checkOutDate,
        int? minPrice,
        int? maxPrice)
    {
        return ViewComponent("TourCards", new
        {
          columnClass,
            ShowHeading = false,
            take = 8,
            DestinationId,
            checkInDate,
            checkOutDate,
            minPrice,
            maxPrice
        });
    }



  }
}
