using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.ViewComponents
{
    public class PackageListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiUrlOptions _apiOptions;
        private readonly string _baseUrl;

        public PackageListViewComponent(IHttpClientFactory clientFactory, ApiUrlOptions apiOptions)
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

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var client = _clientFactory.CreateClient();
            var apiUrl = $"{_baseUrl}/api/Destination/getAllSpots?id={id}";

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
