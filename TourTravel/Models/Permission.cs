using System.ComponentModel.DataAnnotations.Schema;

namespace TourTravel.Models
{
  public class Permission:Base
  {
    public string RoleId { get; set; } = null!;
    public int PageId { get; set; }
    public virtual Role? Role { get; set; }
    public virtual Page? Page { get; set; }
  }

}
