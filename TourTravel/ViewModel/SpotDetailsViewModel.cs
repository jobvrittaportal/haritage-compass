namespace TourTravel.ViewModel
{


  public class SpotGalleryViewModel
  {
    public string ImageFileName { get; set; }
    public bool? IsCoverPhoto { get; set; }
    public string ImageUrl { get; set; }
    public string CoverImageUrl { get; set; }
  }

  public class SpotDetailsViewModel
  {
    public int Id { get; set; }
    public int CityId { get; set; }
    public int SpotId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string CoverPhoto { get; set; }
    public List<SpotGalleryViewModel> Gallery { get; set; } = new();
  }
}
