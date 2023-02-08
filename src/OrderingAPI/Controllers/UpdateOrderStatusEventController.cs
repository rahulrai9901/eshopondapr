using System;
using Actors;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderingAPI.Model;

namespace OrderingAPI.Controllers;

public class UpdateOrderStatusEventController : ControllerBase
{
    private const string DAPR_PUBSUB_NAME = "eshopondapr-pubsub";

    private readonly ILogger<UpdateOrderStatusEventController> _logger;

    public UpdateOrderStatusEventController(ILogger<UpdateOrderStatusEventController> logger)
	{
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    [HttpPost("OrderStatusChangedToSubmitted")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToSubmittedIntegrationEvent integrationEvent)
    {
        _logger.LogInformation("updating order status to submitted {@integrationEvent}", integrationEvent);
    }
}

