using CatalogService.API.Repositories;

namespace CatalogService.API.Services;
public class CatalogService(ICatalogRepository CatalogRepository) : ICatalogService
{
    private readonly ICatalogRepository _CatalogRepository = CatalogRepository;

    public Task<Catalog> GetCatalogByIdAsync(string id) =>
        _CatalogRepository.GetCatalogByIdAsync(id);

    public Task<IEnumerable<Catalog>> GetCatalogsAsync() =>
        _CatalogRepository.GetCatalogsAsync();

    public Task CreateCatalogAsync(Catalog Catalog) =>
        _CatalogRepository.CreateCatalogAsync(Catalog);
}
