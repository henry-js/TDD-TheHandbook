using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence;

public class RoomBookingAppDbContext : DbContext
{

    public RoomBookingAppDbContext(DbContextOptions<RoomBookingAppDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomBooking> RoomBookings { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RoomBookingAppDb;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Conference Room 1" },
            new Room { Id = 2, Name = "Conference Room 2" },
            new Room { Id = 3, Name = "Conference Room 3" }
        );
    }
}