using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Mappers;
using InventoryMicroservice.Core.Result;
using InventoryMicroservice.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace InventoryMicroservice.Core.Services;
public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IDistributedCache _cache;
    public InventoryService(IInventoryRepository inventoryRepo, IDistributedCache cache)
    {
        _inventoryRepo = inventoryRepo;
        _cache = cache; 
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

        await _cache.RemoveAsync($"inventoryItem:{sku}");

        return Result<InventoryItemResponse>.Success(inventoryItem.ToInventoryItemResponse());
    }

    public async Task<Result<InventoryItemResponse>> GetInventoryBySku(string sku)
    {
        string cacheKey = $"inventoryItem:{sku}";
        string? responseCache = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(responseCache))
        {
            InventoryItemResponse? inventoryItemFromCache = JsonConvert.DeserializeObject<InventoryItemResponse>(responseCache);
            return Result<InventoryItemResponse>.Success(inventoryItemFromCache!);
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            return Result<InventoryItemResponse>.Failure("Sku is null or invalid",StatusCode.BadRequest);
        }

        InventoryItem? inventoryItem = await _inventoryRepo.GetInventoryBySku(sku);

        if (inventoryItem == null)
        {
            return Result<InventoryItemResponse>.Failure("InventoryItem NotFound", StatusCode.NotFound);
        }

        string InventoryItemInJson = JsonConvert.SerializeObject(inventoryItem);
        await _cache.SetStringAsync(cacheKey,InventoryItemInJson, new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2.5)));

        return Result<InventoryItemResponse>.Success(inventoryItem.ToInventoryItemResponse());
    }
}
