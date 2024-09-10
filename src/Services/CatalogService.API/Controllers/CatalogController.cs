using CatalogService.API.Dtos;
using CatalogService.API.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CatalogService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly Serilog.ILogger _logger;

    public CatalogController(ICatalogService catalogService, Serilog.ILogger logger)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Catalog>> GetCatalog(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            _logger.Warning("Catalog ID is null or empty.");
            return BadRequest("Catalog ID must be provided.");
        }

        _logger.Information("Fetching catalog with ID: {Id}", id);

        try
        {
            var catalog = await _catalogService.GetCatalogByIdAsync(id);
            if (catalog == null)
            {
                _logger.Warning("Catalog with ID: {Id} not found", id);
                return NotFound($"Catalog with ID: {id} not found.");
            }

            _logger.Information("Catalog with ID: {Id} found", id);
            return Ok(catalog);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while fetching the catalog with ID: {Id}", id);
            return StatusCode(500, "Internal server error occurred while fetching the catalog.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Catalog>>> GetCatalogs()
    {
        _logger.Information("Fetching all catalogs");

        try
        {
            var catalogs = await _catalogService.GetCatalogsAsync();
            _logger.Information("Fetched {Count} catalogs", catalogs.Count());

            return Ok(catalogs);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while fetching catalogs.");
            return StatusCode(500, "Internal server error occurred while fetching catalogs.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Catalog>> CreateCatalog([FromBody] CreateCatalogDto createCatalogDto)
    {
        if (createCatalogDto == null)
        {
            _logger.Warning("Catalog creation data is null.");
            return BadRequest("Catalog creation data must be provided.");
        }

        if (!ModelState.IsValid)
        {
            _logger.Warning("Invalid catalog creation data: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        _logger.Information("Creating catalog with Name: {Name}", createCatalogDto.Name);

        try
        {
            var createdCatalog = await _catalogService.CreateCatalogAsync(createCatalogDto);
            _logger.Information("Catalog with ID: {Id} created successfully", createdCatalog.Id);

            return CreatedAtAction(nameof(GetCatalog), new { id = createdCatalog.Id }, createdCatalog);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating the catalog.");
            return StatusCode(500, "Internal server error occurred while creating the catalog.");
        }
    }
}
