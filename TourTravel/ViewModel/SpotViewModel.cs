namespace TourTravel.ViewModel
{
  public class SpotViewModel
  {
    public int Id { get; set; }
    public int CityId { get; set; }
    public int SpotId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
  }
}
