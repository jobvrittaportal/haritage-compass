namespace TourTravel.ViewModel
{
  public class DestinationDropdownViewModel
  {
    public List<DestinationDropdown> ?Destinations { get; set; } = new();
    public int? SelectedDestinationId { get; set; }
  }
}
