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
    public OrderService(InventoryMicroserviceClient inventoryClient, ProductMicroserviceClient productClient, IOrderRepository orderRepo, IRabbitMQPublisher publisher)
    {
        _inventoryClient = inventoryClient;
        _productClient = productClient;
        _orderRepo = orderRepo;
        _publisher = publisher;
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

        if(createdOrder == null)
        {
            return Result<OrderResponse>.Failure($"Order not created", StatusCode.BadRequest);
        }

        _publisher.Publish("order.created", new OrderCreateMessage(request.Items,order.Id.ToString(),order.CreatedAt,order.CustomerName,order.OrderNumber));
        
        return Result<OrderResponse>.Success(createdOrder.ToOrderResponse());
    }

    public async Task<Result<bool>> DeleteOrder(Guid id)
    {
        bool isDeleted = await _orderRepo.DeleteOrder(id);
        if (!isDeleted) {

            return Result<bool>.Failure("Order with given id not exitst",StatusCode.NotFound);
        }
        return Result<bool>.Success(isDeleted);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrders()
    {
        IEnumerable<Order> orders = await _orderRepo.GetAllOrders();
        return orders.Select(item => item.ToOrderResponse());
    }

    public async Task<Result<OrderResponse>> GetOrderById(Guid id)
    {
        Order? order = await _orderRepo.GetOrderById(id);
        if(order == null)
        {
            return Result<OrderResponse>.Failure("Order with given id not exist", StatusCode.NotFound);
        }
        return Result<OrderResponse>.Success(order.ToOrderResponse());
    }

    public async Task<Result<string>> GetOrderStatusById(Guid id)
    {
        string? orderStatus = await _orderRepo.GetOrderStatusById(id);
        if(orderStatus == null)
        {
            return Result<string>.Failure("Order with given id not exitst", StatusCode.NotFound);
        }
        return Result<string>.Success(orderStatus);
    }
}
