using System.ComponentModel.DataAnnotations;

namespace TourTravel.Models
{
  public class Page:Base
  {
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsFeature { get; set; } = false;
  }

}
