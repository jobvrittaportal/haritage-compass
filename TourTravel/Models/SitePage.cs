namespace TourTravel.Models
{
  public class SitePage :Base
  {
    public required string Page { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string Keywords { get; set; }
    public string Image { get; set; }
    public string ImageHeight { get; set; }
    public string ImageWidth { get; set; }
    public string ImageType { get; set; }
  }
}
