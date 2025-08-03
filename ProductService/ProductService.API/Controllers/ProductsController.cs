using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProductService.Core.DTO;
using ProductService.Core.Result;
using ProductService.Core.ServiceContracts;


namespace ProductService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;
    private readonly IProductService _productService;

    public ProductsController(IDistributedCache distributedCache, IProductService productService)
    {
        _distributedCache = distributedCache;
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
        if (result.IsSuccess == false)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return Ok(result.Value!);
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(Guid id, ProductUpdateRequest product)
    {
        Result<ProductUpdateRequest> result = await _productService.UpdateProductAsync(product, id);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> PostProduct(ProductAddRequest product)
    {
        Result<ProductResponse> result = await _productService.AddProductAsync(product);

        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }

        return CreatedAtAction("GetProductById", new { id = result.Value!.Id });
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        Result<ProductResponse> result = await _productService.DeleteProduct(id);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message,statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }
}
