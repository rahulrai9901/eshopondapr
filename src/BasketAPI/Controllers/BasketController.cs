namespace Basket.Controller;

[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepo _basketRepo;
    private readonly ILogger<BasketController> _logger;
     public BasketController(ILogger<BasketController> logger, IBasketRepo basketRepo)
    {
        _basketRepo = basketRepo;
        _logger = logger;

    }

    
    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> Get([FromRoute] string userId)
    {
        var basket = await _basketRepo.GetBasketAsync(userId);
        return Ok(basket);
    
    }

    // DELETE api/values/5
    [HttpDelete]
    [Route("{userId}")]
    public async Task DeleteBasketAsync([FromRoute]string userId)
    {

        _logger.LogInformation("Deleting basket for user {UserId}...", userId);

        await _basketRepo.DeleteBasketAsync(userId);
    }

    [HttpPost]
    [Route("{userId}")]
    public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value, [FromRoute]string userId)
    {

        value.BuyerId = userId;

        return Ok(await _basketRepo.UpdateBasketAsync(value));
    }

    [HttpPost]
    [Route("user/{userId}")]
    public async Task<ActionResult<CustomerBasket>> CreateBasketAsync([FromRoute]string userId)
    {
        _basketRepo.CreateBasketAsync(userId);
        return Ok();
    }
}