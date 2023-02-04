namespace BasketAPI.Service;
public class BasketRepo : IBasketRepo
{
    private const string StateStoreName = "eshopondapr-statestore";

    private readonly DaprClient _daprClient;
    private readonly ILogger _logger;

    public BasketRepo(DaprClient daprClient, ILogger<BasketRepo> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public Task DeleteBasketAsync(string id) =>
        _daprClient.DeleteStateAsync(StateStoreName, id);

    public Task<CustomerBasket> GetBasketAsync(string customerId) =>
        _daprClient.GetStateAsync<CustomerBasket>(StateStoreName, customerId);

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var state = await _daprClient.GetStateEntryAsync<CustomerBasket>(StateStoreName, basket.BuyerId);
        state.Value = basket;

        await state.SaveAsync();

        _logger.LogInformation("Basket item persisted successfully.");

        return await GetBasketAsync(basket.BuyerId);
    }

    public async Task CreateBasketAsync(string userId)
    {
        try {
        await _daprClient.SaveStateAsync<CustomerBasket>(
            StateStoreName, userId, new CustomerBasket());
        }
        catch(Exception ex){
        _logger.LogInformation("Basket item persisted successfully.");
        }
    }
}
