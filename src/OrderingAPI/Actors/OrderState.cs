namespace Actors;

public class OrderState
{
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Description { get; set; }
    public OrderAddressState Address { get; set; }
    public string BuyerId { get; set; }
    public string BuyerEmail { get; set; }
    public List<OrderItemState> OrderItems { get; set; }
    public decimal GetTotal() => OrderItems.Sum(o => o.Units * o.UnitPrice);
}