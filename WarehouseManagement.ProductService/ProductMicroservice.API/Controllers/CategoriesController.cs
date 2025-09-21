using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Results;
using ProductMicroservice.Core.ServiceContracts;

namespace ProductMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories(CancellationToken cancellationToken,[FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 20);

            PagedResult<CategoryResponse> result = await _categoryService.GetCategoriesAsync(page,pageSize,cancellationToken);

            if (result.TotalCount.HasValue)
            {
                Response.Headers["X-Total-Count"] = result.TotalCount.Value.ToString();
            }

            return Ok(result.Items);
        }

        // GET: api/Categories/5
        [ProducesResponseType(typeof(CategoryResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            Result<CategoryResponse> response = await _categoryService.GetCategoryAsync(id, cancellationToken);
            if (!response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }

            return Ok(response.Value);
        }

        // PUT: api/Categories/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status409Conflict)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PutCategory(Guid id, [FromBody] CategoryUpdateRequest category,CancellationToken cancellationToken)
        {
            Result result = await _categoryService.UpdateCategoryNameAsync(id,category, cancellationToken);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> PostCategory(CategoryAddRequest category)
        {
            Result<CategoryResponse> response = await _categoryService.AddCategory(category);
            if (!response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }

            return CreatedAtAction("GetCategory", new { id = response.Value!.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            Result<bool> response = await _categoryService.DeleteCategory(id);
            if (!response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }

            return NoContent();
        }

        [HttpGet("relatedProducts/{id}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetRelatedProductsWithCategoryById(Guid id)
        {
            Result<IEnumerable<ProductResponse>> response = await _categoryService.GetRelatedProductsWithCategoryById(id);
            if (!response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }

            return Ok(response.Value);
        }
    }
}
