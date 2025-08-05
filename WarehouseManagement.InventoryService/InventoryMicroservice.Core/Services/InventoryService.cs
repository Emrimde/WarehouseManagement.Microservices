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
