using Continuous_Learning_Booking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Continuous_Learning_Booking.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Booking> Bookings { get; set; }
    }

}
