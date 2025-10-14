using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

public class MyDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
  public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

  public DbSet<Page> Pages { get; set; } = null!;
  public DbSet<Permission> Permissions { get; set; } = null!;
  public DbSet<Blog> Blog { get; set; }
  public DbSet<Testimonial> Testimonial { get; set; }
  public DbSet<TourCardsView> TourCardsView { get; set; }
  public DbSet<TourGuideView> TourGuideView { get; set; }
  public DbSet<About> About { get; set; }
  public DbSet<FAQ> FAQ { get; set; }
  public DbSet<TermsOfService> TermsOfService { get; set; }
  public DbSet<PrivacyPolicy> PrivacyPolicy { get; set; }
  public DbSet<Gallery> Gallery { get; set; }
  public DbSet<Destinations> Destinations { get; set; }
  public DbSet<DestinationGallery> DestinationGallery { get; set; }
  public DbSet<State> State { get; set; }
  public DbSet<City> City { get; set; }
  public DbSet<Country> Country { get; set; }
  public DbSet<Package> Package { get; set; }

}
