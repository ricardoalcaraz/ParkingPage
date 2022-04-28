using Microsoft.EntityFrameworkCore;

namespace ParkingPageApi;

public class ParkingPageContext : DbContext
{
    public ParkingPageContext(DbContextOptions<ParkingPageContext> options) : base(options)
    {
        
    }
    
    public DbSet<Metric> Metrics { get; set; }
}

public class Metric
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsReachable { get; set; }
}