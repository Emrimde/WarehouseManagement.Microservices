using OrderMicroservice.Core.Domain.Entities;
using OrderMicroservice.Core.DTO.OrderDTO;
using OrderMicroservice.Core.DTO.ProductDTO;

namespace OrderMicroservice.Core.Mappers;

public static class OrderItemMappers
{
    public static OrderItem ToOrderItem(string sku, string productName, int quantity, int unitPrice)
    {
        return new OrderItem()
        {
            ProductName = productName,
            SKU = sku,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
    public static Order ToOrder(List<ProductResponse> productList, OrderAddRequest order)
    {
        return new Order()
        {
            Items = productList.Select((item, index) => new OrderItem()
            {
                ProductName = item.Name,
                Quantity = order.Items[index].Quantity,
                SKU = order.Items[index].SKU,
                UnitPrice = item.UnitPrice

            }).ToList(),
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
        };
    }

    public static OrderResponse ToOrderResponse(this Order order)
    {
        return new OrderResponse()
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status
        };
    }
}
