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
        private readonly ApiUrlOptions _apiOptions;
        private readonly string _baseUrl;

        public FeaturedToursViewComponent(IHttpClientFactory clientFactory, ApiUrlOptions apiOptions)
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/api/package/getIsFeatureList");

            if (!response.IsSuccessStatusCode)
                return View(new List<FeaturedTourDto>());

            var json = await response.Content.ReadAsStringAsync();

            var featuredTours = JsonSerializer.Deserialize<List<FeaturedTourDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(featuredTours);
        }
    }
}
