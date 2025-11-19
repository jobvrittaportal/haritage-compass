using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TourTravel.Models;

namespace TourTravel.ViewComponents
{
    public class DestinationDropdownViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiUrlOptions _apiOptions;
        private readonly string _baseUrl;

        public DestinationDropdownViewComponent(IHttpClientFactory clientFactory, ApiUrlOptions apiOptions)
        {
            _clientFactory = clientFactory;
            _apiOptions = apiOptions;
            _baseUrl = apiOptions.Use switch
            {
                "Live" => apiOptions.Live,
                "Stage" => apiOptions.Stage,
                "Local" => apiOptions.Local,
                _ => apiOptions.Live
            };
        }

        public async Task<IViewComponentResult> InvokeAsync(int? SelectedDestinationId)
        {
            List<DestinationDropdown> destinations = new();

            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri($"{_baseUrl}/api/");
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
            ViewBag.Baseurl = _baseUrl;
            ViewBag.SelectedDestinationId = SelectedDestinationId;
            return View(destinations); // Views/Shared/Components/DestinationDropdown/Default.cshtml
        }
    }

    public class DestinationDropdown
    {
        public int Id { get; set; }
        public string Destination { get; set; }
    }
}
