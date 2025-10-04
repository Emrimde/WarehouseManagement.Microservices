using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Enums;
using ProductMicroservice.Core.Results;
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

    // GET: api/Products?page=1&pageSize=10
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PagedResult<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ProductResponse>>> GetProducts([FromQuery] string? name, CancellationToken cancellationToken, [FromQuery] int page = 1,[FromQuery] int pageSize = 10, [FromQuery] ProductSearchCategoriesEnum category = ProductSearchCategoriesEnum.None, [FromQuery] bool showActive = true)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 30); 

        PagedResult<ProductResponse> paged = await _productService.GetProductsPagedAsync(page, pageSize,name,category, cancellationToken,showActive);

        return Ok(paged);
    }


    // GET: api/Products/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> Get(Guid id, CancellationToken cancellationToken)
    { 
        if(id == Guid.Empty)
        {
            return BadRequest();
        }

        Result<ProductResponse> result = await _productService.GetProductByIdAsync(id, cancellationToken);
        if (!result.IsSuccess)
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

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductUpdateRequest product, CancellationToken cancellationToken)
    {
        Result result = await _productService.UpdateProductAsync(product, id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }

    // POST: api/Products
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> PostProduct([FromBody] ProductAddRequest product, CancellationToken cancellationToken)
    {
        Result<ProductCreateResponse> result = await _productService.AddProductAsync(product, cancellationToken);

        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }

        return CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value);
    }

    // DELETE: api/Products/5
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id:guid}")]

    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        Result result = await _productService.DeleteProductAsync(id,cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message,statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:guid}/permanent")]
    public async Task<ActionResult> PermanentDelete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        Result result = await _productService.PermanentDeleteAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<ActionResult> RestoreProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        Result result = await _productService.RestoreProductAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return NoContent();
    }
}
