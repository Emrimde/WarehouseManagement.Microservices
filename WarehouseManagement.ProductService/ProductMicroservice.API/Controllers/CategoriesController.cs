using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Result;
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

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            IEnumerable<CategoryResponse> categories = await _categoryService.GetCategories();
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse>> GetCategory(Guid id)
        {
            Result<CategoryResponse> response = await _categoryService.GetCategoryById(id);
            if (!response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }

            return Ok(response.Value);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, [FromBody] CategoryUpdateRequest category)
        {
            Result<CategoryResponse> result = await _categoryService.UpdateCategory(id,category);
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
