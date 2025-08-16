using OrderMicroservice.Core.Domain.Entities;
using OrderMicroservice.Core.Domain.RepositoryContracts;
using OrderMicroservice.Core.DTO;
using OrderMicroservice.Core.DTO.InventoryDTO;
using OrderMicroservice.Core.DTO.ProductDTO;
using OrderMicroservice.Core.HttpClients;
using OrderMicroservice.Core.Result;
using OrderMicroservice.Core.ServiceContracts;

namespace OrderMicroservice.Core.Services;
public class OrderService : IOrderService
{
    private readonly InventoryMicroserviceClient _inventoryClient;
    private readonly ProductMicroserviceClient _productClient;
    private readonly IOrderRepository _orderRepo;
    public OrderService(InventoryMicroserviceClient inventoryClient, ProductMicroserviceClient productClient, IOrderRepository orderRepo)
    {
        _inventoryClient = inventoryClient;
        _productClient = productClient;
        _orderRepo = orderRepo;
    }
    public async Task<Result<bool>> AddOrder(OrderAddRequest request)
    {
        List<ProductResponse> productResponseList = new();
        if (request == null || request.Items == null || !request.Items.Any())
        {
            return Result<bool>.Failure("Request must contain at least one item.", StatusCode.BadRequest);
        }

        foreach (OrderItemAddRequest item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.SKU))
            {
                return Result<bool>.Failure("SKU is invalid", StatusCode.BadRequest);
            }
            if (item.Quantity <= 0)
            {
                return Result<bool>.Failure("Quantity must be greater than zero", StatusCode.BadRequest);
            }
        }

        foreach (OrderItemAddRequest item in request.Items)
        {

            InventoryItemResponse? inventoryItem = await _inventoryClient.GetInventoryBySkuAsync(item.SKU);
            if (inventoryItem == null)
            {
                return Result<bool>.Failure($"Inventory item for given sku not found", StatusCode.NotFound);
            }

            if (inventoryItem.Available < item.Quantity)
            {
                return Result<bool>.Failure($"Not enough quantity in the magazine", StatusCode.Conflict);
            }

            ProductResponse? product = await _productClient.GetProductBySkuAsync(item.SKU);
            if (product == null)
            {
                return Result<bool>.Failure($"product for given sku not found", StatusCode.NotFound);
            }
            product.UnitPrice = inventoryItem.UnitPrice;

            productResponseList.Add(product);
        }

        Order order = new()
        {
            Items = productResponseList.Select((item, index) => new OrderItem()
            {
                SKU = request.Items[index].SKU,
                Quantity = request.Items[index].Quantity,
                ProductName = item.Name,
                UnitPrice = item.UnitPrice,

            }).ToList(),
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
        };

        order.Items.ToList().ForEach(item => item.Order = order);

        Order createdOrder = await _orderRepo.AddOrder(order);
        return Result<bool>.Success(true);
    }

}
