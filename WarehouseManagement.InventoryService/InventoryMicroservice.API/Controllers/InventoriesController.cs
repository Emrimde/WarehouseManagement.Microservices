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

    public InventoriesController(ApplicationDbContext context,IInventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    // GET: api/Inventories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryBySku(string sku)
    {
        Result<InventoryItemResponse> result = await _inventoryService.GetInventoryBySku(sku);
        if (!result.IsSuccess)
        {
            return Problem(detail: result.Message, statusCode: (int)result.StatusCode);
        }
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventories()
    {
        return await _context.InventoryItems.ToListAsync();
    }

    // PUT: api/InventoryItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInventoryItem(Guid id, InventoryItem inventoryItem)
    {
        if (id != inventoryItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(inventoryItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InventoryItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/InventoryItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<InventoryItem>> PostInventoryItem(InventoryItem inventoryItem)
    {
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetInventoryItem", new { id = inventoryItem.Id }, inventoryItem);
    }

    // DELETE: api/InventoryItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryItem(Guid id)
    {
        var inventoryItem = await _context.InventoryItems.FindAsync(id);
        if (inventoryItem == null)
        {
            return NotFound();
        }

        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool InventoryItemExists(Guid id)
    {
        return _context.InventoryItems.Any(e => e.Id == id);
    }
}
