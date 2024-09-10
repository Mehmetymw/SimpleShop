using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BasketService.API.Dtos;
public class CreateBasketItemDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string CatalogId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero"),DefaultValue(1)]
    public int Quantity { get; set; }
}
