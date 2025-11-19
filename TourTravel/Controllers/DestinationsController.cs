using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TourTravel.Models;
using TourTravel.ViewModel;

namespace TourTravel.Controllers
{
    public class DestinationsController : Controller
    {
        private readonly MyDbContext db;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiUrlOptions _apiOptions;
        private readonly string _baseUrl;

        public DestinationsController(IHttpClientFactory clientFactory, MyDbContext db, ApiUrlOptions apiOptions)
        {
            this.db = db;
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

        public async Task<IActionResult> Index(int currentPage = 1)
        {

            var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
            ViewBag.CurrentPage = currentPage;

            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }





        [HttpGet]
        public async Task<IActionResult> DestinationDetails(int id)
        {
            ViewBag.Page = "Destination Detail";

            var page = db.SitePages.FirstOrDefault(f => f.Page == "Destinations");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }

            var viewModel = new DestinationDetailsViewModel();

            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri($"{_baseUrl}/api/");


                var response = await client.GetFromJsonAsync<DestinationDetailsViewModel>(
                    $"Destination/getDestinationDetails?id={id}");

                if (response != null)
                {
                    viewModel.Id = response.Id;
                    viewModel.CityName = response.CityName ?? response.Name ?? "Explore Destination";
                    viewModel.CityDescription = response.CityDescription ?? response.Description;
                    viewModel.CityCoverImage = response.CityCoverImage ?? response.CoverPhoto;
                    viewModel.Gallery = response.Gallery ?? new List<ImageDto>();
                    viewModel.MapUrl = "https://www.google.com/maps/embed?pb=...";
                    viewModel.Latitude = response.Latitude ?? 0;
                    viewModel.Longitude = response.Longitude ?? 0;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching destination details: {ex.Message}");
            }

            return View(viewModel);
        }


        public async Task<IActionResult> SpotDetails(int id)
        {
            ViewBag.Page = "Spot Detail";

            var client = _clientFactory.CreateClient();
            var apiUrl = $"{_baseUrl}/api/Destination/getSpotsDetails?id={id}";


            List<SpotDetailsViewModel>? spots = null;

            try
            {
                spots = await client.GetFromJsonAsync<List<SpotDetailsViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching spot details: {ex.Message}");
            }

            var spot = spots?.FirstOrDefault();

            if (spot == null)
                return NotFound();

            return View(spot);
        }



    }


    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsTouristDestination { get; set; }
        public bool IsDisplayHome { get; set; }
        public string ImageFileName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCoverPhoto { get; set; }
        public int? OrderIndex { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }



}
