using System;
using OrderingAPI.Model;

namespace OrderingAPI.Infrastructure.Repository;

public interface IOrderRepository
{
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<Order?> GetOrderByOrderNumberAsync(int orderNumber);
    Task<Order> AddOrGetOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<IEnumerable<OrderSummary>> GetOrdersFromBuyerAsync(string buyerId);
}

