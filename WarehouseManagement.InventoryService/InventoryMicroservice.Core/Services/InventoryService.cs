using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Mappers;
using InventoryMicroservice.Core.Result;
using InventoryMicroservice.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace InventoryMicroservice.Core.Services;
public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IDistributedCache _cache;
    private readonly ILogger<InventoryService> _logger; 
    public InventoryService(IInventoryRepository inventoryRepo, IDistributedCache cache,ILogger<InventoryService> logger)
    {
        _inventoryRepo = inventoryRepo;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<InventoryItemResponse>> AdjustInventoryItem(string sku, InventoryUpdateRequest request,CancellationToken cancellationToken)
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
        InventoryItem? inventoryItem = await _inventoryRepo.AdjustInventoryItem(sku, inventoryItemFromUpdateRequest,cancellationToken);

        if (inventoryItem == null)
        {
            return Result<InventoryItemResponse>.Failure("Inventory item not found", StatusCode.NotFound);
        }

        await _cache.RemoveAsync($"inventoryItem:{sku}");

        return Result<InventoryItemResponse>.Success(inventoryItem.ToInventoryItemResponse());
    }

    public async Task<PagedResult<InventoryItemResponse>> GetAllInventories(int page, int pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<InventoryItem> inventoryItems = await _inventoryRepo.GetAllInventoryItems(page, pageSize, cancellationToken);

        IEnumerable<InventoryItemResponse> results = inventoryItems.Select(item => item.ToInventoryItemResponse());

        PagedResult<InventoryItemResponse> response = new PagedResult<InventoryItemResponse>()
        {
            Page = page,
            PageSize = pageSize,
            Items = results,
            TotalCount = await _inventoryRepo.GetInventoryItemsCount()
        };

        return response;
    }

    public async Task<Result<InventoryItemResponse>> GetInventoryBySkuAsync(string sku,CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return Result<InventoryItemResponse>.Failure("Sku is null or invalid",StatusCode.BadRequest);
        }

        string cacheKey = $"inventoryItem:{sku}";
        byte[]? responseCache = await _cache.GetAsync(cacheKey,cancellationToken);

        if (responseCache != null && responseCache.Length > 1)
        {
            InventoryItemResponse? inventoryItemFromCache = JsonSerializer.Deserialize<InventoryItemResponse>(responseCache);
            return Result<InventoryItemResponse>.Success(inventoryItemFromCache!);
        }

        InventoryItem? inventoryItem = null; 
        try
        {
            inventoryItem = await _inventoryRepo.GetInventoryBySkuAsync(sku,cancellationToken);



        }
        catch(OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("{functionName} function cancelled for sku {sku} ", nameof(GetInventoryBySkuAsync), sku);
        }

        if (inventoryItem == null)
        {
            return Result<InventoryItemResponse>.Failure("InventoryItem NotFound", StatusCode.NotFound);
        }
        string InventoryItemInJson = JsonSerializer.Serialize(inventoryItem);
        byte[]? inventoryItemInBytes = Encoding.UTF8.GetBytes(InventoryItemInJson);
        await _cache.SetAsync(cacheKey, inventoryItemInBytes, new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetSlidingExpiration(TimeSpan.FromMinutes(15)),cancellationToken);
        return Result<InventoryItemResponse>.Success(inventoryItem.ToInventoryItemResponse());
    }
}
