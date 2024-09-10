using BasketService.API.Models;

namespace BasketService.API.Repositories;
public interface IBasketRepository
{
    Task<IEnumerable<BasketItem>> GetBasketAsync(string userId);
    Task AddToBasketAsync(BasketItem item);
    Task UpdateBasketAsync(BasketItem item);
    Task RemoveFromBasketAsync(string userId, string itemId);
}