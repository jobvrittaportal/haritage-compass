using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourTravel.Models
{
    public class State : Base
    {
        [MaxLength(255)]
        public required string Name { get; set; }
        public bool IsTouristDestination { get; set; } = false;
        public required int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }
        public virtual List<City>? Cities { get; set; }
    }
}
