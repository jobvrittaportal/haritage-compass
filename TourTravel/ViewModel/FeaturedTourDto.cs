namespace TourTravel.ViewModel
{
  public class FeaturedTourDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public string PackageImage { get; set; }
    public decimal BasePrice { get; set; }
    public int MaxPerson { get; set; }
    public bool IsFeature { get; set; }
  }
}
