using TourTravel.Controllers;

namespace TourTravel.ViewModel
{
  //public class DestinationDetailsViewModel
  //{
  //  public string CityName { get; set; }
  //  public string CityCoverImage { get; set; }
  //  public string MapUrl { get; set; }
  //  public List<SpotDetailDto> Spots { get; set; } = new();
  //}

  //public class SpotDetailDto
  //{
  //  public int Id { get; set; }
  //  public int CityId { get; set; }
  //  public int SpotId { get; set; }
  //  public string Title { get; set; }
  //  public string ImageUrl { get; set; }
  //  public string Description { get; set; }
  //  public bool IsActive { get; set; }
  //  public bool IsCoverPhoto { get; set; }
  //  public int? OrderIndex { get; set; }
  //}
  public class DestinationDetailsViewModel
  {
    public int Id { get; set; }

    public string CityName { get; set; }
    public string Name { get; set; }
    public string CityDescription { get; set; }
    public string Description { get; set; }

    public string CityCoverImage { get; set; }
    public string CoverPhoto { get; set; }

    public List<ImageDto> Gallery { get; set; } = new List<ImageDto>();

    public string MapUrl { get; set; }
  }

  public class ImageDto
  {
    public string ImageFileName { get; set; }
    public bool? IsCoverPhoto { get; set; }
    public int? OrderIndex { get; set; }

    public string ThumbUrl { get; set; }
    public string MediumUrl { get; set; }
  }


}
