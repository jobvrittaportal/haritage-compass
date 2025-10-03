using TourTravel.Models;
using Microsoft.EntityFrameworkCore;

namespace TourTravel.Data
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> User { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Testimonial> Testimonial { get; set; }
        public DbSet<TourCardsView> TourCardsView { get; set; }
        public DbSet<TourGuideView> TourGuideView { get; set; }
    }
}
