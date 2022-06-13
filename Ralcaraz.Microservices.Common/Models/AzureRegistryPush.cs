namespace Ralcaraz.Microservices.Common.Models;

public record AzureRegistryPush
{
    public string Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Action { get; set; }
    public RegistryTarget? Target { get; set; }
    public RegistryRequest? Request { get; set; }
}

public record RegistryTarget
{
    public string MediaType { get; set; }
    public int Size { get; set; }
    public string Digest { get; set; }
    public int Length { get; set; }
    public string? Registry { get; set; }
    public string? Tag { get; set; }
}

public record RegistryRequest
{
    public string Id { get; set; }
    public string? Host { get; set; }
    public string? Method { get; set; }
    public string? UserAgent { get; set; }
}