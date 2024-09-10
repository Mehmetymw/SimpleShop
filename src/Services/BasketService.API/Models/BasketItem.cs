namespace BasketService.API.Models;
public class BasketItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = "test"; //User Service olmadığı için default bir kullanıcı olduğunu varsayıyorum.
    public string CatalogId { get; set; }
    public int Quantity { get; set; }
}