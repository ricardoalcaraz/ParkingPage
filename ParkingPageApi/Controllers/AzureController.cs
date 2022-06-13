using MediatR;
using Ralcaraz.Microservices.Common.Models;

namespace ParkingPageApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AzureController : ControllerBase
{
    private readonly ILogger<AzureController> _logger;
    private readonly IMediator _mediator;

    public AzureController(ILogger<AzureController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpPost("prod")]
    public async Task<IActionResult> ProdWebHook([FromBody] AzureRegistryPush pushEvent, CancellationToken ctx)
    {
        _logger.LogInformation("Received push event: {Event}", pushEvent);
        IRequest pushCommand = pushEvent.Target.Tag switch
        {
            "dev" => new AzureDevPush(),
            "latest" => new AzureProdPush(),
            _ => new AzureProdPush()
        };

        await _mediator.Send(pushCommand, ctx);
        
        return Ok();
    }
}

