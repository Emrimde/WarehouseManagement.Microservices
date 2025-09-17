using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Result;
using ProductMicroservice.Core.ServiceContracts;

namespace ProductMicroservice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
    {
        IEnumerable<ProductResponse> products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    // GET: api/Products?page=1&pageSize=10
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PagedResult<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ProductResponse>>> GetProducts([FromQuery] int page = 1,[FromQuery] int pageSize = 10,CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 30); // limit do 100

        PagedResult<ProductResponse> paged = await _productService.GetProductsPagedAsync(page, pageSize, cancellationToken);

        if (paged.TotalCount.HasValue)
            Response.Headers["X-Total-Count"] = paged.TotalCount.Value.ToString();

        return Ok(paged);
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

    // GET: api/Products/sku/{sku}
    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<ProductResponse>> GetProductBySku(string sku)
    {
        Result<ProductResponse> result = await _productService.GetProductBySkuAsync(sku);
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
    public async Task<ActionResult<ProductResponse>> PostProduct([FromBody] ProductAddRequest product)
    {
        Result<ProductResponse> result = await _productService.AddProductAsync(product);

        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }

        return CreatedAtAction("GetProductById", new { id = result.Value!.Id }, result.Value);
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
