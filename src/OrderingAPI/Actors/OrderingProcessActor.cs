using System;
using System.Collections.Generic;
using Dapr;
using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;
using Dapr.Client;
using eShopEvent;
using Microsoft.Extensions.Options;
using OrderingAPI.Actors;
using OrderingAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Actors;

public class OrderingProcessActor : Actor, IOrderingProcessActor, IRemindable
{

    private const string OrderDetailsStateName = "OrderDetails";
    private const string OrderStatusStateName = "OrderStatus";

    private const string GracePeriodElapsedReminder = "GracePeriodElapsed";
    private const string StockConfirmedReminder = "StockConfirmed";
    private const string StockRejectedReminder = "StockRejected";
    private const string PaymentSucceededReminder = "PaymentSucceeded";
    private const string PaymentFailedReminder = "PaymentFailed";

    private readonly IEventPublish _eventBus;
    private readonly IOptions<OrderingSettings> _settings;

    private int? _preMethodOrderStatusId;

    public OrderingProcessActor(ActorHost host, IEventPublish evenpublish, IOptions<OrderingSettings> settings) : base(host)
    {
        _eventBus = evenpublish;
        _settings = settings;
    }

    private Guid OrderId => Guid.Parse(Id.GetId());

    public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        throw new NotImplementedException();
    }

    public async Task SubmitAsync(string buyerId, string buyerEmail, string street, string city, string state, string country, CustomerBasket basket)
    {
        var orderState = new OrderState
        {
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.Submitted,
            Description = "Submitted",
            Address = new OrderAddressState
            {
                Street = street,
                City = city,
                State = state,
                Country = country
            },
            BuyerId = buyerId,
            BuyerEmail = buyerEmail,
            OrderItems = basket.Items
               .Select(item => new OrderItemState
               {
                   ProductId = item.ProductId,
                   ProductName = item.ProductName,
                   UnitPrice = item.UnitPrice,
                   Units = item.Quantity,
                   PictureFileName = item.PictureFileName
               })
               .ToList()
        };

        await StateManager.SetStateAsync(OrderDetailsStateName, orderState);
        await StateManager.SetStateAsync(OrderStatusStateName, OrderStatus.Submitted);

        await RegisterReminderAsync(
            GracePeriodElapsedReminder,
            null,
            TimeSpan.FromSeconds(_settings.Value.GracePeriodTime),
            TimeSpan.FromMilliseconds(-1));

        await _eventBus.PublishAsync(new OrderStatusChangedToSubmittedIntegrationEvent(
            OrderId,
            OrderStatus.Submitted.Name,
            buyerId,
            buyerEmail));
    }
}

