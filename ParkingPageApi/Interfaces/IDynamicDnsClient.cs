namespace ParkingPageApi.Interfaces;

public interface IDynamicDnsClient
{
    public Task<bool> TryUpdate(UpdateDynamicDns dynamicDn, CancellationToken ctx);
}