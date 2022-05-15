namespace ParkingPageApi.Models;

public class ParkingPageContext : DbContext
{
    public ParkingPageContext(DbContextOptions<ParkingPageContext> options) : base(options)
    {
    }

    public DbSet<DdnsUpdate> DdnsUpdates { get; set; } = null!;
}