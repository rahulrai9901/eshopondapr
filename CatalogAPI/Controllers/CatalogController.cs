

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using CatalogAPI.Infrastructure;
using CatalogAPI.Model;

namespace CatalogAPI.Controllers;

public class CatalogController : ControllerBase
{
    //"test"
    private readonly CatalogDbContext _context;

    public CatalogController(CatalogDbContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    [HttpGet("brands")]
    [ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
    public Task<List<CatalogBrand>> CatalogBrandsAsync() =>
       _context.CatalogBrands.ToListAsync();

    [HttpGet("types")]
    [ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
    public Task<List<CatalogType>> CatalogTypesAsync() =>
        _context.CatalogTypes.ToListAsync();
}

