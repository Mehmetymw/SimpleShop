using CatalogService.API.Dtos;
using CatalogService.API.Models;

namespace CatalogService.API.Services;
public interface ICatalogService
{
    Task<Catalog> GetCatalogByIdAsync(string id);
    Task<IEnumerable<Catalog>> GetCatalogsAsync();
    Task<Catalog> CreateCatalogAsync(CreateCatalogDto Catalog);
}
