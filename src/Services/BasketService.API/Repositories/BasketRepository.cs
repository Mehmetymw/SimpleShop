using System.Text.Json;
using BasketService.API.Models;
using StackExchange.Redis;

namespace BasketService.API.Repositories;
public class BasketRepository(IConnectionMultiplexer redis, Serilog.ILogger logger) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();
    private readonly Serilog.ILogger _logger = logger;

    public async Task<IEnumerable<BasketItem>> GetBasketAsync(string userId)
    {
        _logger.Information("Fetching basket for user with ID: {UserId}", userId);

        try
        {
            var basket = await _database.StringGetAsync(userId);
            var result = string.IsNullOrEmpty(basket) ? [] : JsonSerializer.Deserialize<IEnumerable<BasketItem>>(basket).Where(t=>t.Quantity!=0);

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

    public async Task RemoveFromBasketAsync(string userId, string itemId)
    {
        _logger.Information("Removing item with ID: {ItemId} from basket for user with ID: {UserId}", itemId, userId);

        try
        {
            var basket = await GetBasketAsync(userId);
            var updatedBasket = basket.Where(b => b.Id != itemId);
            await _database.StringSetAsync(userId, JsonSerializer.Serialize(updatedBasket));

            _logger.Information("Item with ID: {ItemId} removed from basket for user with ID: {UserId} successfully", itemId, userId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while removing item with ID: {ItemId} from basket for user with ID: {UserId}", itemId, userId);
            throw;
        }
    }
}
