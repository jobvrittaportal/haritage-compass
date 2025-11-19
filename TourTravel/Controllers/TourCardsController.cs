using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TourTravel.Models;

namespace TourTravel.Controllers
{
    public class TourCardsController : Controller
    {
        private readonly MyDbContext db;
        private readonly ApiUrlOptions _apiOptions;
        private readonly string _baseUrl;
        public TourCardsController(MyDbContext db, ApiUrlOptions apiOptions)
        {
            this.db = db;
            _apiOptions = apiOptions;
            _baseUrl = apiOptions.Use switch
            {
                "Live" => apiOptions.Live,
                "Stage" => apiOptions.Stage,
                "Local" => apiOptions.Local,
                _ => apiOptions.Live
            };
        }
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
