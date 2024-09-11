using CatalogService.API.Configurations;
using CatalogService.API.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace CatalogService.API.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly IMongoCollection<Catalog> _catalogs;
    private readonly Serilog.ILogger _logger;

    public CatalogRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings, Serilog.ILogger logger)
    {
        if (mongoSettings == null || mongoSettings.Value == null)
        {
            throw new ArgumentNullException(nameof(mongoSettings), "MongoSettings cannot be null.");
        }

        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _catalogs = database.GetCollection<Catalog>("Catalog");
        _logger = logger;

        _logger.Information("CatalogRepository initialized with database: {DatabaseName}", mongoSettings.Value.DatabaseName);
    }


    public async Task<Catalog> GetCatalogByIdAsync(string id)
    {
        _logger.Information("Fetching catalog with ID: {CatalogId}", id);

        try
        {
            var catalog = await _catalogs.Find(p => p.Id.ToString() == id).FirstOrDefaultAsync();
            if (catalog == null)
            {
                _logger.Warning("Catalog with ID: {CatalogId} not found", id);
            }

            return catalog;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while fetching catalog with ID: {CatalogId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Catalog>> GetCatalogsAsync()
    {
        _logger.Information("Fetching all catalogs");

        try
        {
            return await _catalogs.Find(p => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while fetching catalogs");
            throw;
        }
    }

    public async Task<Catalog> CreateCatalogAsync(CreateCatalogDto createCatalogDto)
    {
        _logger.Information("Creating catalog with name: {CatalogName}", createCatalogDto.Name);

        var catalog = new Catalog
        {
            Id = Guid.NewGuid(), //Bson Oid'e çekilebilir.
            Name = createCatalogDto.Name,
            Description = createCatalogDto.Description,
            Price = createCatalogDto.Price
        };

        try
        {
            await _catalogs.InsertOneAsync(catalog);
            _logger.Information("Catalog with ID: {CatalogId} created successfully", catalog.Id);
            return catalog;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while creating catalog with name: {CatalogName}", createCatalogDto.Name);
            throw;
        }
    }
}
