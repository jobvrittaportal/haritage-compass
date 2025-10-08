namespace TourTravel.Models
{
  public class RolePermissionViewModel
  {
    public int PageId { get; set; }
    public string PageName { get; set; } = null!;
    public bool IsAssigned { get; set; } = false;
  }

}
