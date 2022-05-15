// ReSharper disable once CheckNamespace
namespace ParkingPageApi.Options;

public record DynamicDnsSetting(string Domain, string Password, IEnumerable<string> Hosts);

public record UpdateDynamicDns(string Ip, string Domain, string Host, string Password);