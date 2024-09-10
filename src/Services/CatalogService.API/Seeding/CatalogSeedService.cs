using CatalogService.API.Dtos;
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

        var seedItems = new List<CreateCatalogDto>
        {
            new () { Name = "Item 1", Price = 10.0m, Description = "Description for Item 1" },
            new () { Name = "Item 2", Price = 20.0m, Description = "Description for Item 2" },
            new () { Name = "Item 3", Price = 30.0m, Description = "Description for Item 3" },
            new () { Name = "Item 4", Price = 40.0m, Description = "Description for Item 4" },
            new () { Name = "Item 5", Price = 50.0m, Description = "Description for Item 5" },
            new () { Name = "Item 6", Price = 60.0m, Description = "Description for Item 6" },
            new () { Name = "Item 7", Price = 70.0m, Description = "Description for Item 7" },
            new () { Name = "Item 8", Price = 80.0m, Description = "Description for Item 8" },
            new () { Name = "Item 9", Price = 90.0m, Description = "Description for Item 9" },
            new () { Name = "Item 10", Price = 100.0m, Description = "Description for Item 10" },
            new () { Name = "Item 11", Price = 110.0m, Description = "Description for Item 11" },
            new () { Name = "Item 12", Price = 120.0m, Description = "Description for Item 12" },
            new () { Name = "Item 13", Price = 130.0m, Description = "Description for Item 13" },
            new () { Name = "Item 14", Price = 140.0m, Description = "Description for Item 14" },
            new () { Name = "Item 15", Price = 150.0m, Description = "Description for Item 15" },
            new () { Name = "Item 16", Price = 160.0m, Description = "Description for Item 16" },
            new () { Name = "Item 17", Price = 170.0m, Description = "Description for Item 17" },
            new () { Name = "Item 18", Price = 180.0m, Description = "Description for Item 18" },
            new () { Name = "Item 19", Price = 190.0m, Description = "Description for Item 19" },
            new () { Name = "Item 20", Price = 200.0m, Description = "Description for Item 20" }
        };
        foreach (var item in seedItems)
        {
            await _catalogRepository.CreateCatalogAsync(item);
        }
    }
}