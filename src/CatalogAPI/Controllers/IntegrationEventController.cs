using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogAPI.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogAPI.Controllers;

public class IntegrationEventController : ControllerBase
{
    private ILogger<IntegrationEventController> _logger;
    private const string DAPR_PUBSUB_NAME = "eshopondapr-pubsub";

    public IntegrationEventController(ILogger<IntegrationEventController> logger)
    {
        _logger = logger;
    }

    //[HttpPost("UserCheckoutAccepted")]
    //[Topic(DAPR_PUBSUB_NAME, "UserCheckoutAcceptedIntegrationEvent")]
    //public async Task HandleAsync(UserCheckoutAcceptedIntegrationEvent integrationEvent)
    //{
    //    _logger.LogInformation("Recieved UserCheckoutAcceptedIntegrationEvent - ", integrationEvent);
    //}
}

