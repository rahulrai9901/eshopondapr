using System;
namespace OrderingAPI.Model;

public record OrderSummary(
    Guid Id,
    int OrderNumber,
    DateTime OrderDate,
    string OrderStatus,
    decimal Total);
