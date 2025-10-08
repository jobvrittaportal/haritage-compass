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
}
