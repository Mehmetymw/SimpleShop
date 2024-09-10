using CatalogService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _CatalogService;

    public CatalogController(ICatalogService CatalogService)
    {
        _CatalogService = CatalogService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Catalog>> GetCatalog(string id)
    {
        var Catalog = await _CatalogService.GetCatalogByIdAsync(id);
        if (Catalog == null)
        {
            return NotFound();
        }
        return Ok(Catalog);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Catalog>>> GetCatalogs()
    {
        var Catalogs = await _CatalogService.GetCatalogsAsync();
        return Ok(Catalogs);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCatalog(Catalog Catalog)
    {
        await _CatalogService.CreateCatalogAsync(Catalog);
        return CreatedAtAction(nameof(GetCatalog), new { id = Catalog.Id }, Catalog);
    }
}
