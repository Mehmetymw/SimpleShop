using CatalogService.API.Dtos;
using CatalogService.API.Repositories;

namespace CatalogService.API.Services;
public class CatalogService(ICatalogRepository CatalogRepository) : ICatalogService
{
    private readonly ICatalogRepository _CatalogRepository = CatalogRepository;

    public async Task<Catalog> GetCatalogByIdAsync(string id) =>
        await _CatalogRepository.GetCatalogByIdAsync(id);

    public async Task<IEnumerable<Catalog>> GetCatalogsAsync() =>
        await _CatalogRepository.GetCatalogsAsync();

    public async Task<Catalog> CreateCatalogAsync(CreateCatalogDto Catalog) =>
        await _CatalogRepository.CreateCatalogAsync(Catalog);
}
