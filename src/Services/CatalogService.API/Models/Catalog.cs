using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.API.Models;
public class Catalog
{
    [BsonId]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}

