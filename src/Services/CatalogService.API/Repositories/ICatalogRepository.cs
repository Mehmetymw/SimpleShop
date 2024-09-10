namespace CatalogService.API.Repositories;
public interface ICatalogRepository
{
    Task<Catalog> GetCatalogByIdAsync(string id);
    Task<IEnumerable<Catalog>> GetCatalogsAsync();
    Task CreateCatalogAsync(Catalog Catalog);
}
