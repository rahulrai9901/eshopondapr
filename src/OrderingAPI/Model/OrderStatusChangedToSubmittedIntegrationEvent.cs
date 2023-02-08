using eShopEvent;
namespace OrderingAPI.Model;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId,
    string BuyerEmail) : IntegrationEvent;