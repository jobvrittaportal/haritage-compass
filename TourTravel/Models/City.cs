using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourTravel.Models
{
    public class City : Base
    {

        [MaxLength(255)]
        public required string Name { get; set; }
        public bool IsTouristDestination { get; set; } = false;
        public required int StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
    }
}
