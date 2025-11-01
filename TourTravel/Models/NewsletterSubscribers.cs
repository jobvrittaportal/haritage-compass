namespace TourTravel.Models
{
    public class NewsletterSubscribers : Base
    {
        public string Email { get; set; }
        public bool IsSubscribed { get; set; } = true;
    }
}
