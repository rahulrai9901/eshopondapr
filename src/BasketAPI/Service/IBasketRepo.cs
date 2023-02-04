namespace BasketAPI.Service;
using BasketAPI.Model;
public interface IBasketRepo
{
    Task<CustomerBasket> GetBasketAsync(string customerId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task DeleteBasketAsync(string id);
    Task CreateBasketAsync(string userId);

}