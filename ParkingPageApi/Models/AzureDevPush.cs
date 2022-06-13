using MediatR;

namespace ParkingPageApi.Models;

public record AzureDevPush : IRequest;

public record AzureProdPush : IRequest;

public class AzurePushHandler : IRequestHandler<AzureDevPush>, IRequestHandler<AzureProdPush>
{
    public Task<Unit> Handle(AzureProdPush request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }

    public Task<Unit> Handle(AzureDevPush request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }
}