using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Result;
using InventoryMicroservice.Core.ServiceContracts;
using InventoryMicroservice.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMicroservice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IInventoryService _inventoryService;

    public InventoriesController(ApplicationDbContext context, IInventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    // GET: api/Inventories/5
    [HttpGet("{sku}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryBySku(string sku)
    {
        Result<InventoryItemResponse> result = await _inventoryService.GetInventoryBySku(sku);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventories()
    {
        return await _context.InventoryItems.ToListAsync();
    }

    // POST: api/InventoryItems/{sku}/adjust
    [HttpPost("{sku}/adjust")]
    public async Task<IActionResult> AdjustInventoryQuantityOnHand(string sku, [FromBody] int adjustment)
    {
        Result<InventoryItemResponse> response = await _inventoryService.AdjustQuantityOnHand(sku, adjustment);
        if (!response.IsSuccess)
        {
            return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
        }

        return Ok();
    }
}
