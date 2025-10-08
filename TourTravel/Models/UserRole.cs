using System.ComponentModel.DataAnnotations.Schema;

namespace TourTravel.Models
{
  public class UserRole : Base
  {
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
  }
}
