using System;
namespace OrderingAPI.Model;

public class CustomerBasket
{
    public string BuyerId { get; set; } = string.Empty;

    public List<BasketItem> Items { get; set; } = new();
}
