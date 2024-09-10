using CatalogService.API.Dtos;

namespace CatalogService.API.Repositories;
public interface ICatalogRepository
{
    Task<Catalog> GetCatalogByIdAsync(string id);
    Task<IEnumerable<Catalog>> GetCatalogsAsync();
    Task<Catalog> CreateCatalogAsync(CreateCatalogDto createCatalogDto);
}
