using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actors;
using Dapr;
using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;
using OrderingAPI.Actors;
using OrderingAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderingAPI.Controllers;

public class OrderingProcessingEventController : Controller
{
    private const string DAPR_PUBSUB_NAME = "eshopondapr-pubsub";
    private IActorProxyFactory _actorProxyFactory;
    private ILogger<OrderingProcessingEventController> _logger;

    public OrderingProcessingEventController(
       IActorProxyFactory actorProxyFactory,
       ILogger<OrderingProcessingEventController> logger)
    {
        _actorProxyFactory = actorProxyFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("UserCheckoutAccepted")]
    [Topic(DAPR_PUBSUB_NAME, "UserCheckoutAcceptedIntegrationEvent")]
    public async Task HandleAsync(UserCheckoutAcceptedIntegrationEvent integrationEvent)
    {
        _logger.LogInformation("recieved user chekout accepted - {@IntegrationEvent}", integrationEvent);
        if (integrationEvent.RequestId != Guid.Empty)
        {
            var orderingProcess = GetOrderingProcessActor(integrationEvent.RequestId);

            await orderingProcess.SubmitAsync(
                integrationEvent.UserId, integrationEvent.UserEmail, integrationEvent.Street, integrationEvent.City,
                integrationEvent.State, integrationEvent.Country, integrationEvent.Basket);
        }
        else
        {
            _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", integrationEvent);
        }
    }

    private IOrderingProcessActor GetOrderingProcessActor(Guid orderId)
    {
        var actorId = new ActorId(orderId.ToString());
        return _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
            actorId,
            nameof(OrderingProcessActor));
    }
}

