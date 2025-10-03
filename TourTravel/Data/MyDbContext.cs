using TourTravel.Models;
using Microsoft.EntityFrameworkCore;

namespace TourTravel.Data
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
    public DbSet<User> User { get; set; }

  }
}
