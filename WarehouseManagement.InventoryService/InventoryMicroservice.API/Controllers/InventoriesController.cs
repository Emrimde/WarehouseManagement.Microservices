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
    /// <summary>
    /// Retrieves the current inventory state for a specific product identified by SKU.
    /// </summary>
    /// <param name="sku">The SKU (Stock Keeping Unit) code of the product.</param>
    /// <returns>Inventory details of the specified product.</returns>
    [HttpGet("{sku}")]
    public async Task<ActionResult<InventoryItemResponse>> GetInventoryBySku(string sku)
    {
        Result<InventoryItemResponse> result = await _inventoryService.GetInventoryBySku(sku);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return Ok(result.Value);
    }

    /// <summary>
    /// Retrieves the inventory details for all products.
    /// </summary>
    /// <returns>List of inventory items.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventories()
    {
        return await _context.InventoryItems.ToListAsync();
    }

    // POST: api/InventoryItems/{sku}/adjust
    /// <summary>
    /// Adjusts the available quantity for a specific product.
    /// </summary>
    /// <param name="sku">The SKU (Stock Keeping Unit) code of the product to update.</param>
    /// <param name="adjustment">
    /// The quantity change to apply. Positive values increase stock, negative values decrease it.
    /// </param>
    /// <returns>No content if successful, or an error message if the operation fails.</returns>
    [HttpPatch("{sku}/adjust")]
    public async Task<IActionResult> AdjustInventory(string sku, [FromBody] InventoryUpdateRequest request)
    {
        Result<InventoryItemResponse> response = await _inventoryService.AdjustInventoryItem(sku, request);
        if (!response.IsSuccess)
        {
            return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
        }

        return Ok(response.Value);
    }
}
