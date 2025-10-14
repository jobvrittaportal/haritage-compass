namespace TourTravel.ViewModel
{
    public class DestinationDetailsViewModel
    {
        public TourTravel.Models.Destinations Destination { get; set; } = null!;                    // used for Main model (all dest info)
        public List<TourTravel.Models.DestinationGallery> GalleryImages { get; set; } = new();      // used for destination gallery
    }
}
