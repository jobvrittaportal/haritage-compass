namespace TourTravel.Models
{
  public class About :Base
  {
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Image { get; set; }
    public required string KeyFeatures { get; set; }
    public required string MetaTitle { get; set; }
    public required string MetaDescription { get; set; }
    public string? SlugUrl { get; set; }
  }
}
