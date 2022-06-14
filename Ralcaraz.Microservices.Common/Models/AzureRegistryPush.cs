namespace Ralcaraz.Microservices.Common.Models;

public record AzureRegistryPush
{
    public string Id { get; init; }
    public DateTime TimeStamp { get; init; }
    public string Action { get; init; }
    public RegistryTarget? Target { get; init; }
    public RegistryRequest? Request { get; init; }
}

public record RegistryTarget
{
    public string MediaType { get; init; }
    public int Size { get; init; }
    public string Digest { get; init; }
    public int Length { get; init; }
    public string? Repository { get; init; }
    public string? Tag { get; init; }
}

public record RegistryRequest
{
    public string Id { get; init; }
    public string? Host { get; init; }
    public string? Method { get; init; }
    public string? UserAgent { get; init; }
}