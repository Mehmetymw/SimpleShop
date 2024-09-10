
using BasketService.API.Models;
using BasketService.API.Repositories;

namespace BasketService.API.Services; 
public class BasketService(IBasketRepository basketRepository) : IBasketService
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<IEnumerable<BasketItem>> GetBasketAsync(string userId)
    {
        return await _basketRepository.GetBasketAsync(userId);
    }

    public async Task AddToBasketAsync(BasketItem item)
    {
        await _basketRepository.AddToBasketAsync(item);
    }

    public async Task UpdateBasketAsync(BasketItem item)
    {
        await _basketRepository.UpdateBasketAsync(item);
    }

    public async Task RemoveFromBasketAsync(string userId, string itemId)
    {
        await _basketRepository.RemoveFromBasketAsync(userId, itemId);
    }
}