using OrderMicroservice.Core.Domain.Entities;
using OrderMicroservice.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMicroservice.Core.Mappers;

public static class OrderItemMappers
{
    public static OrderItem ToOrder(string sku, string productName, int quantity, int unitPrice)
    {
        return new OrderItem()
        {
            ProductName = productName,
            SKU = sku,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}
