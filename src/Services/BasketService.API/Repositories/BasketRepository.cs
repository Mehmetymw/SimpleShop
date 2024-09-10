using System.Text.Json;
using BasketService.API.Models;
using StackExchange.Redis;

namespace BasketService.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly Serilog.ILogger _logger;

        public BasketRepository(IConnectionMultiplexer redis, Serilog.ILogger logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<IEnumerable<BasketItem>> GetBasketAsync(string userId)
        {
            _logger.Information("Fetching basket for user with ID: {UserId}", userId);

            try
            {
                var basket = await _database.StringGetAsync(userId);
                var result = string.IsNullOrEmpty(basket) 
                    ? Enumerable.Empty<BasketItem>() 
                    : JsonSerializer.Deserialize<IEnumerable<BasketItem>>(basket);

                _logger.Information("Basket for user with ID: {UserId} retrieved successfully with {Count} items", userId, result.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while fetching basket for user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task AddToBasketAsync(BasketItem item)
        {
            _logger.Information("Adding item with ID: {ItemId} to basket for user with ID: {UserId}", item.Id, item.UserId);

            try
            {
                var basketItems = await GetBasketAsync(item.UserId);
                var basketItemList = basketItems.ToList();

                var existingItem = basketItemList.FirstOrDefault(x => x.CatalogId == item.CatalogId);

                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    basketItemList.Add(item);
                }

                await _database.StringSetAsync(item.UserId, JsonSerializer.Serialize(basketItemList));

                _logger.Information("Item with ID: {ItemId} added to basket successfully for user with ID: {UserId}", item.Id, item.UserId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding item with ID: {ItemId} to basket for user with ID: {UserId}", item.Id, item.UserId);
                throw;
            }
        }

        public async Task UpdateBasketAsync(BasketItem item)
        {
            _logger.Information("Updating item with ID: {ItemId} in basket for user with ID: {UserId}", item.Id, item.UserId);

            try
            {
                var basketItems = await GetBasketAsync(item.UserId);
                var basketItemList = basketItems.ToList();

                var existingItem = basketItemList.FirstOrDefault(x => x.CatalogId == item.CatalogId);

                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                }
                else
                {
                    _logger.Warning("Item with CatalogId: {CatalogId} not found in the basket for user with ID: {UserId}", item.CatalogId, item.UserId);
                    throw new KeyNotFoundException($"Item with CatalogId: {item.CatalogId} not found in the basket.");
                }

                await _database.StringSetAsync(item.UserId, JsonSerializer.Serialize(basketItemList));

                _logger.Information("Item with ID: {ItemId} updated successfully in basket for user with ID: {UserId}", item.Id, item.UserId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating item with ID: {ItemId} in basket for user with ID: {UserId}", item.Id, item.UserId);
                throw;
            }
        }

        public async Task RemoveFromBasketAsync(string userId, string catalogId)
        {
            _logger.Information("Removing item with CatalogId: {CatalogId} from basket for user with ID: {UserId}", catalogId, userId);

            try
            {
                var basketItems = await GetBasketAsync(userId);
                var basketItemList = basketItems.ToList();

                var itemToRemove = basketItemList.FirstOrDefault(x => x.CatalogId == catalogId);

                if (itemToRemove != null)
                {
                    basketItemList.Remove(itemToRemove);
                    await _database.StringSetAsync(userId, JsonSerializer.Serialize(basketItemList));
                    
                    _logger.Information("Item with CatalogId: {CatalogId} removed successfully from basket for user with ID: {UserId}", catalogId, userId);
                }
                else
                {
                    _logger.Warning("Item with CatalogId: {CatalogId} not found in the basket for user with ID: {UserId}", catalogId, userId);
                    throw new KeyNotFoundException($"Item with CatalogId: {catalogId} not found in the basket.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while removing item with CatalogId: {CatalogId} from basket for user with ID: {UserId}", catalogId, userId);
                throw;
            }
        }
    }
}
