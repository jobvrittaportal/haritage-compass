using System.ComponentModel.DataAnnotations.Schema;

namespace TourTravel.Models
{
  public class Package:Base
  {
    public required string Name { get; set; }
    public required int Duration { get; set; } 
    public int? CityId { get; set; }
    [ForeignKey(nameof(CityId))]
    public virtual City? City { get; set; }
    public string? Desc { get; set; }
    public bool IsActive { get; set; } = true;
    public required decimal Price { get; set; }
    public int? ReviewsCount { get; set; }
    public required int MaxPerson { get; set; }
    public string? ImageUrl { get; set; }
    public required string MetaTitle { get; set; }
    public required string MetaDescription { get; set; }
    public string? SlugUrl { get; set; }
  }
}
