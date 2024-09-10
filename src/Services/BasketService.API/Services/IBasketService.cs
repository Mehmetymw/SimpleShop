using BasketService.API.Dtos;
using BasketService.API.Models;

namespace BasketService.API.Services;

 public interface IBasketService
    {
        Task<IEnumerable<BasketItem>> GetBasketAsync(string userId);
        Task<BasketItem> CreateBasketItemAsync(CreateBasketItemDto item);
        Task UpdateBasketItemAsync(BasketItem item);
        Task RemoveBasketItemAsync(string userId, string itemId);
    }