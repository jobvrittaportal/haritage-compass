using Microsoft.AspNetCore.Identity;
namespace TourTravel.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
  }
}
