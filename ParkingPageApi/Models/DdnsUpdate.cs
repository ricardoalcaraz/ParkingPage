namespace ParkingPageApi.Models;

public class DdnsUpdate
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Ip { get; set; } = null!;
    public string ServerName { get; set; } = null!;
}