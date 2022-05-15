namespace ParkingPageApi;

public class ParkingPageContext : DbContext
{
    public ParkingPageContext(DbContextOptions<ParkingPageContext> options) : base(options)
    {
    }

    public DbSet<DdnsUpdate> DdnsUpdates { get; set; } = null!;
}

public class DdnsUpdate
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Ip { get; set; } = null!;
    public string ServerName { get; set; } = null!;
}