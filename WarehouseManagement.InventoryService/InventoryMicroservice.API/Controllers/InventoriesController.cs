using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Result;
using InventoryMicroservice.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMicroservice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoriesController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // GET: api/Inventories/5
    [ProducesResponseType(typeof(InventoryItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{sku}")]
    public async Task<ActionResult<InventoryItemResponse>> GetInventoryBySku(string sku,CancellationToken cancellationToken)
    {
        Result<InventoryItemResponse> result = await _inventoryService.GetInventoryBySkuAsync(sku,cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return Ok(result.Value);
    }

    [ProducesResponseType(typeof(IEnumerable<InventoryItemResponse>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventories([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 20);

        PagedResult<InventoryItemResponse> result = await _inventoryService.GetAllInventories(page,pageSize,cancellationToken);

        return Ok(result);
    }

    // POST: api/Invetories/{sku}/adjust
    [HttpPatch("{sku}/adjust")]
    public async Task<IActionResult> AdjustInventory(string sku, [FromBody] InventoryUpdateRequest request, CancellationToken cancellationToken)
    {
        Result<InventoryItemResponse> response = await _inventoryService.AdjustInventoryItem(sku, request,cancellationToken);
        if (!response.IsSuccess)
        {
            return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
        }

        return Ok(response.Value);
    }
}
