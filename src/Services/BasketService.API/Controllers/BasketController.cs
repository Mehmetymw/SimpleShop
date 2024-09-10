using BasketService.API.Dtos;
using BasketService.API.Models;
using BasketService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IBasketService basketService, Serilog.ILogger logger) : ControllerBase
    {
        private readonly IBasketService _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        private readonly Serilog.ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasket(string userId)
        {
            _logger.Information("Request received to fetch basket for user with ID: {UserId}", userId);

            try
            {
                var basketItems = await _basketService.GetBasketAsync(userId);
                if (basketItems == null || !basketItems.Any())
                {
                    _logger.Warning("No items found in the basket for user with ID: {UserId}", userId);
                    return NotFound("No items found in the basket.");
                }

                _logger.Information("Successfully fetched {ItemCount} items from the basket for user with ID: {UserId}", basketItems.Count(), userId);
                return Ok(basketItems);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while fetching the basket for user with ID: {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BasketItem>> CreateBasketItem([FromBody] CreateBasketItemDto item)
        {
            if (item == null)
            {
                _logger.Warning("CreateBasketItem called with null item");
                return BadRequest("Item cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(item.UserId) || string.IsNullOrWhiteSpace(item.CatalogId))
            {
                _logger.Warning("CreateBasketItem called with invalid item: {Item}", item);
                return BadRequest("UserId and CatalogId cannot be null or empty.");
            }

            try
            {
                var newItem = await _basketService.CreateBasketItemAsync(item);
                _logger.Information("Successfully created basket item with ID: {ItemId} for user with ID: {UserId}", newItem.Id, item.UserId);
                return CreatedAtAction(nameof(GetBasket), new { userId = item.UserId }, newItem);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while creating basket item for user with ID: {UserId}", item.UserId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBasketItem([FromBody] BasketItem item)
        {
            if (item == null)
            {
                _logger.Warning("UpdateBasketItem called with null item");
                return BadRequest("Item cannot be null.");
            }

            try
            {
                await _basketService.UpdateBasketItemAsync(item);
                _logger.Information("Successfully updated basket item with ID: {ItemId}", item.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while updating basket item with ID: {ItemId}", item.Id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userId}/{itemId}")]
        public async Task<ActionResult> RemoveBasketItem(string userId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(itemId))
            {
                _logger.Warning("RemoveBasketItem called with null or empty userId or itemId");
                return BadRequest("UserId and ItemId cannot be null or empty.");
            }

            try
            {
                await _basketService.RemoveBasketItemAsync(userId, itemId);
                _logger.Information("Successfully removed basket item with ID: {ItemId} for user with ID: {UserId}", itemId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while removing basket item with ID: {ItemId} for user with ID: {UserId}", itemId, userId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
