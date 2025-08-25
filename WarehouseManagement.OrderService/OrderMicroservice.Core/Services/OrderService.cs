using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OrderMicroservice.Core.Domain.Entities;
using OrderMicroservice.Core.Domain.RepositoryContracts;
using OrderMicroservice.Core.DTO.InventoryDTO;
using OrderMicroservice.Core.DTO.OrderDTO;
using OrderMicroservice.Core.DTO.ProductDTO;
using OrderMicroservice.Core.HttpClients;
using OrderMicroservice.Core.Mappers;
using OrderMicroservice.Core.RabbitMQ;
using OrderMicroservice.Core.Result;
using OrderMicroservice.Core.ServiceContracts;

namespace OrderMicroservice.Core.Services;
public class OrderService : IOrderService
{
    private readonly InventoryMicroserviceClient _inventoryClient;
    private readonly IRabbitMQPublisher _publisher;
    private readonly ProductMicroserviceClient _productClient;
    private readonly IOrderRepository _orderRepo;
    private readonly IDistributedCache _cache;
    public OrderService(InventoryMicroserviceClient inventoryClient, ProductMicroserviceClient productClient, IOrderRepository orderRepo, IRabbitMQPublisher publisher, IDistributedCache cache)
    {
        _inventoryClient = inventoryClient;
        _productClient = productClient;
        _orderRepo = orderRepo;
        _publisher = publisher;
        _cache = cache;
    }
    public async Task<Result<OrderResponse>> AddOrder(OrderAddRequest request)
    {
        List<ProductResponse> productResponseList = new();
        if (request == null || request.Items == null || !request.Items.Any())
        {
            return Result<OrderResponse>.Failure("Request must contain at least one item.", StatusCode.BadRequest);
        }

        foreach (OrderItemAddRequest item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.SKU))
            {
                return Result<OrderResponse>.Failure("SKU is invalid", StatusCode.BadRequest);
            }
            if (item.Quantity <= 0)
            {
                return Result<OrderResponse>.Failure("Quantity must be greater than zero", StatusCode.BadRequest);
            }
        }

        foreach (OrderItemAddRequest item in request.Items)
        {

            InventoryItemResponse? inventoryItem = await _inventoryClient.GetInventoryBySkuAsync(item.SKU);
            if (inventoryItem == null)
            {
                return Result<OrderResponse>.Failure($"Inventory item for given sku not found", StatusCode.NotFound);
            }

            if (inventoryItem.Available < item.Quantity)
            {
                return Result<OrderResponse>.Failure($"Not enough quantity in the magazine", StatusCode.Conflict);
            }

            ProductResponse? product = await _productClient.GetProductBySkuAsync(item.SKU);
            if (product == null)
            {
                return Result<OrderResponse>.Failure($"product for given sku not found", StatusCode.NotFound);
            }
            product.UnitPrice = inventoryItem.UnitPrice;

            productResponseList.Add(product);
        }

        Order order = OrderItemMappers.ToOrder(productResponseList, request);
        order.Items.ToList().ForEach(item => item.Order = order);
        Order createdOrder = await _orderRepo.AddOrder(order);

        if (createdOrder == null)
        {
            return Result<OrderResponse>.Failure($"Order not created", StatusCode.BadRequest);
        }

        _publisher.Publish("order.created", new OrderCreateMessage(request.Items, order.Id.ToString(), order.CreatedAt, order.CustomerName, order.OrderNumber));

        await _cache.RemoveAsync("orders:all");
        return Result<OrderResponse>.Success(createdOrder.ToOrderResponse());
    }

    public async Task<Result<bool>> DeleteOrder(Guid id)
    {
        bool isDeleted = await _orderRepo.DeleteOrder(id);
        if (!isDeleted)
        {

            return Result<bool>.Failure("Order with given id not exitst", StatusCode.NotFound);
        }

        await _cache.RemoveAsync("orders:all");
        await _cache.RemoveAsync($"order:{id}");
        await _cache.RemoveAsync($"orderStatus:{id}");
        return Result<bool>.Success(isDeleted);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrders()
    {
        string cacheKey = "orders:all";
        string? cacheResponse = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            IEnumerable<OrderResponse>? ordersListFromCache = JsonConvert.DeserializeObject<IEnumerable<OrderResponse>>(cacheResponse);
            return ordersListFromCache!;
        }

        IEnumerable<Order> orders = await _orderRepo.GetAllOrders();

        string ordersInJson = JsonConvert.SerializeObject(orders);
        await _cache.SetStringAsync(cacheKey, ordersInJson, new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2.5))); 
        

        return orders.Select(item => item.ToOrderResponse());
    }

    public async Task<Result<OrderResponse>> GetOrderById(Guid id)
    {
        string cacheKey = $"order:{id}";
        string? cacheResponse = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            OrderResponse? orderFromCache = JsonConvert.DeserializeObject<OrderResponse>(cacheResponse);
            return Result<OrderResponse>.Success(orderFromCache!);
        }

        Order? order = await _orderRepo.GetOrderById(id);
        if (order == null)
        {
            return Result<OrderResponse>.Failure("Order with given id not exist", StatusCode.NotFound);
        }

        string orderInJson = JsonConvert.SerializeObject(order);
        await _cache.SetStringAsync(cacheKey, orderInJson, new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2.5)));

        return Result<OrderResponse>.Success(order.ToOrderResponse());
    }

    public async Task<Result<string>> GetOrderStatusById(Guid id)
    {
        string cacheKey = $"orderStatus:{id}";
        string? cacheResponse = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            string? orderStatusFromCache = JsonConvert.DeserializeObject<string>(cacheResponse);
            return Result<string>.Success(orderStatusFromCache!);
        }

        string? orderStatus = await _orderRepo.GetOrderStatusById(id);
        if (orderStatus == null)
        {
            return Result<string>.Failure("Order with given id not exitst", StatusCode.NotFound);
        }

        string orderStatusInJson = JsonConvert.SerializeObject(orderStatus);
        await _cache.SetStringAsync(cacheKey, orderStatusInJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)).SetSlidingExpiration(TimeSpan.FromMinutes(2.5)));

        return Result<string>.Success(orderStatus);
    }
}
