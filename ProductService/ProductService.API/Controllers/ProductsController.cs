using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.Domain.Entities;
using ProductService.Core.DTO;
using ProductService.Core.Result;
using ProductService.Core.ServiceContracts;
using ProductService.Infrastructure.DatabaseContext;

namespace ProductService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;

    public ProductsController(ApplicationDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
    {
        IEnumerable<ProductResponse> products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
    {
        Result<ProductResponse> result = await _productService.GetProductByIdAsync(id);

        if (result.isSuccess == false)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }

        return Ok(result.Value!);
    }

    // PUT: api/Products/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(Guid id, ProductUpdateRequest product)
    {
        Result<ProductResponse> result = await _productService.UpdateProductAsync(product, id);
        if (!result.isSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }

    // POST: api/Products
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(Guid id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
