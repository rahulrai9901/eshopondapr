using System;
namespace eShopEvent;

public interface IEventPublish
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}

