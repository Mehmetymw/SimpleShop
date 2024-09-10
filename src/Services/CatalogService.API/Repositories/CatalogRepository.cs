using CatalogService.API.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace CatalogService.API.Repositories;
public class CatalogRepository : ICatalogRepository
{
    private readonly IMongoCollection<Catalog> _Catalogs;

    public CatalogRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _Catalogs = database.GetCollection<Catalog>("Catalog");
    }

    public async Task<Catalog> GetCatalogByIdAsync(string id) =>
        await _Catalogs.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Catalog>> GetCatalogsAsync() =>
        await _Catalogs.Find(p => true).ToListAsync();

    public async Task CreateCatalogAsync(Catalog Catalog) =>
        await _Catalogs.InsertOneAsync(Catalog);
}
