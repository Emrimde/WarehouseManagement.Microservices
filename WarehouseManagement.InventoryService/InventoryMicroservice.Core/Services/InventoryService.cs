using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Mappers;
using InventoryMicroservice.Core.Result;
using InventoryMicroservice.Core.ServiceContracts;

namespace InventoryMicroservice.Core.Services;
public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepo;
    public InventoryService(IInventoryRepository inventoryRepo)
    {
        _inventoryRepo = inventoryRepo;
    }

    public async Task<Result<InventoryItemResponse>> AdjustInventoryItem(string sku, InventoryUpdateRequest request)
    {
        if (string.IsNullOrEmpty(sku))
        {
            return Result<InventoryItemResponse>.Failure("sku is invalid", StatusCode.BadRequest);
        }

        if(request.QuantityOnHand == 0)
        {
            return Result<InventoryItemResponse>.Failure("Quantity is zero", StatusCode.BadRequest);
        }

        InventoryItem inventoryItemFromUpdateRequest = request.ToInventoryItem();
        InventoryItem? inventoryItem = await _inventoryRepo.AdjustInventoryItem(sku, inventoryItemFromUpdateRequest);

        if (inventoryItem == null)
        {
            return Result<InventoryItemResponse>.Failure("Inventory item not found", StatusCode.NotFound);
        }

        return Result<InventoryItemResponse>.Success(inventoryItem.ToInventoryItemResponse());
    }

    public async Task<Result<InventoryItemResponse>> GetInventoryBySku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return Result<InventoryItemResponse>.Failure("Sku is null or invalid",StatusCode.BadRequest);
        }
        InventoryItem? inventoryItem = await _inventoryRepo.GetInventoryBySku(sku);

        if (inventoryItem == null)
        {
            return Result<InventoryItemResponse>.Failure("InventoryItem NotFound", StatusCode.NotFound);
        }

        return Result<InventoryItemResponse>.Success(inventoryItem.ToInventoryItemResponse());
    }
}
