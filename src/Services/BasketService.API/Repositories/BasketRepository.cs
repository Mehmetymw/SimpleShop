using System.Text.Json;
using BasketService.API.Models;
using BasketService.API.Repositories;
using StackExchange.Redis;
public class BasketRepository : IBasketRepository
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task<IEnumerable<BasketItem>> GetBasketAsync(string userId)
    {
        var basket = await _database.StringGetAsync(userId);
        return string.IsNullOrEmpty(basket) ? [] : JsonSerializer.Deserialize<IEnumerable<BasketItem>>(basket);
    }

    public async Task AddToBasketAsync(BasketItem item)
    {
        var basket = await GetBasketAsync(item.Id);
        var updatedBasket = basket.Append(item);
        await _database.StringSetAsync(item.Id, JsonSerializer.Serialize(updatedBasket));
    }

    public async Task UpdateBasketAsync(BasketItem item)
    {
        var basket = await GetBasketAsync(item.Id);
        var updatedBasket = basket.Where(x => x.Id != item.Id).Append(item);
        await _database.StringSetAsync(item.Id, JsonSerializer.Serialize(updatedBasket));
    }

    public async Task RemoveFromBasketAsync(string userId, string itemId)
    {
        var basket = await GetBasketAsync(userId);
        var updatedBasket = basket.Where(x => x.Id != itemId);
        await _database.StringSetAsync(userId, JsonSerializer.Serialize(updatedBasket));
    }
}