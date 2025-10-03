using System.ComponentModel.DataAnnotations;

namespace TourTravel.Models
{
  public class User : Base
  {
    [MaxLength(255)]
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; } = true;
  }

  public class UserLogin
  {
    public required string Email { get; set; }
    public required string Password { get; set; }
  }
}
