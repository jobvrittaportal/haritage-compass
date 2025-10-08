using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TourTravel.Models
{
  public class Role : IdentityRole
  {
    public string? Desc { get; set; }
  }
}
