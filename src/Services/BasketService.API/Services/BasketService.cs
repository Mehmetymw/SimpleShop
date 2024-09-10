using BasketService.API.Dtos;
using BasketService.API.Models;
using BasketService.API.Repositories;
using Serilog;

namespace BasketService.API.Services
{
    public class BasketService(IBasketRepository basketRepository, Serilog.ILogger logger) : IBasketService
    {
        private readonly IBasketRepository _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        private readonly Serilog.ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<IEnumerable<BasketItem>> GetBasketAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.Warning("GetBasketAsync called with null or empty userId");
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));
            }

            try
            {
                _logger.Information("Fetching basket for user: {UserId}", userId);
                var basket = await _basketRepository.GetBasketAsync(userId);
                _logger.Information("Fetched {Count} items from basket for user: {UserId}", basket.Count(), userId);
                return basket;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching basket for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<BasketItem> CreateBasketItemAsync(CreateBasketItemDto dto)
        {
            if (dto == null)
            {
                _logger.Warning("CreateBasketItemAsync called with null dto");
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.CatalogId))
            {
                _logger.Warning("CreateBasketItemAsync called with invalid dto: {Dto}", dto);
                throw new ArgumentException("UserId and CatalogId cannot be null or empty");
            }

            try
            {
                var newItem = new BasketItem
                {
                    UserId = dto.UserId,
                    CatalogId = dto.CatalogId,
                    Quantity = dto.Quantity
                };

                _logger.Information("Creating basket item with ID: {ItemId} for user: {UserId}", newItem.Id, newItem.UserId);
                await _basketRepository.AddToBasketAsync(newItem);
                _logger.Information("Basket item with ID: {ItemId} created successfully", newItem.Id);
                return newItem;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating basket item for user: {UserId}", dto.UserId);
                throw;
            }
        }

        public async Task UpdateBasketItemAsync(BasketItem item)
        {
            if (item == null)
            {
                _logger.Warning("UpdateBasketItemAsync called with null item");
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                _logger.Information("Updating basket item with ID: {ItemId}", item.Id);
                await _basketRepository.UpdateBasketAsync(item);
                _logger.Information("Basket item with ID: {ItemId} updated successfully", item.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating basket item with ID: {ItemId}", item.Id);
                throw;
            }
        }

        public async Task RemoveBasketItemAsync(string userId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(itemId))
            {
                _logger.Warning("RemoveBasketItemAsync called with null or empty userId or itemId");
                throw new ArgumentException("UserId and ItemId cannot be null or empty");
            }

            try
            {
                _logger.Information("Removing basket item with ID: {ItemId} from user: {UserId}", itemId, userId);
                await _basketRepository.RemoveFromBasketAsync(userId, itemId);
                _logger.Information("Basket item with ID: {ItemId} removed successfully", itemId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing basket item with ID: {ItemId} from user: {UserId}", itemId, userId);
                throw;
            }
        }
    }
}
