using eShopEvent;

namespace Basket.Controller;

[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepo _basketRepo;
    private readonly ILogger<BasketController> _logger;
    private readonly IEventPublish _eventPublish;

    public BasketController(ILogger<BasketController> logger, IBasketRepo basketRepo, IEventPublish eventPublish)
    {
        _basketRepo = basketRepo;
        _logger = logger;
        _eventPublish = eventPublish;

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
        await _basketRepo.CreateBasketAsync(userId);
        return Ok();
    }

    [HttpPost("checkout/{userId}")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CheckoutAsync(
        [FromBody] BasketCheckout basketCheckout,
        [FromHeader(Name = "X-Request-Id")] string requestId, [FromRoute] string userId)
    {

        var basket = await _basketRepo.GetBasketAsync(userId);
        if (basket == null)
        {
            return BadRequest();
        }

        var eventRequestId = Guid.TryParse(requestId, out Guid parsedRequestId)
            ? parsedRequestId : Guid.NewGuid();

        var eventMessage = new UserCheckoutAcceptedIntegrationEvent(
            userId,
            basketCheckout.UserEmail,
            basketCheckout.City,
            basketCheckout.Street,
            basketCheckout.State,
            basketCheckout.Country,
            basketCheckout.CardNumber,
            basketCheckout.CardHolderName,
            basketCheckout.CardExpiration,
            basketCheckout.CardSecurityCode,
            eventRequestId,
            basket);

        // Once basket is checkout, sends an integration event to
        // ordering.api to convert basket to order and proceed with
        // order creation process
        try {
            await _eventPublish.PublishAsync(eventMessage);
        }
        catch(Exception ex){
            _logger.LogError("during publish error came", ex);
        }

        return Accepted();
    }
}