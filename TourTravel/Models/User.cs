using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace TourTravel.Models
{
  using Microsoft.AspNetCore.Identity;
  public class User : IdentityUser
  {
    public string Name { get; set; } = null!;
    public string? EmployeeCode { get; set; }
    public bool IsActive { get; set; } = true;
  }


  public class UserLogin
  {
    public required string Email { get; set; }
    public required string Password { get; set; }
  }
}
