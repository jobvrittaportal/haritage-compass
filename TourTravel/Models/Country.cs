using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TourTravel.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Country : Base
    {
        [MaxLength(255)]
        public required string Name { get; set; }
        public bool IsDestination { get; set; } = false;
        public required string Code { get; set; }
        public required string DialCode { get; set; }
        public virtual List<State>? States { get; set; }
    }
}
