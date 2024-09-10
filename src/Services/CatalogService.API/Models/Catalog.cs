using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.API.Models;
public class Catalog
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}

