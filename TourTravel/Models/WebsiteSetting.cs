namespace TourTravel.Models
{
  public class WebsiteSetting : Base
  {
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? YouTube { get; set; }
    public string? Timing { get; set; }
    public string? Address { get; set; }
    public required int RotationTime { get; set; } =  3;

    }
}
