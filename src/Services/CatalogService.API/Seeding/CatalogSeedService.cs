using CatalogService.API.Repositories;
namespace CatalogService.API.Seeding;
public class CatalogSeedService(ICatalogRepository catalogRepository) : ICatalogSeedService
{
    private readonly ICatalogRepository _catalogRepository = catalogRepository;

    public async Task SeedAsync()
    {
        var existingItems = await _catalogRepository.GetCatalogsAsync();
        if (existingItems.Any())
        {
            return;
        }

        var seedItems = new List<Catalog>
            {
                new(){ Id = Guid.NewGuid().ToString(), Name = "Item 1", Price = 10.0m, Description = "Description for Item 1" },
                new(){ Id = Guid.NewGuid().ToString(), Name = "Item 2", Price = 20.0m, Description = "Description for Item 2" },
                new(){ Id = Guid.NewGuid().ToString(), Name = "Item 3", Price = 30.0m, Description = "Description for Item 3" }
            };

        foreach (var item in seedItems)
        {
            await _catalogRepository.CreateCatalogAsync(item);
        }
    }
}