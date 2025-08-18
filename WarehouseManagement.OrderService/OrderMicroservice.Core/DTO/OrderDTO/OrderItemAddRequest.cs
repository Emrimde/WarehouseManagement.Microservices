using System.Collections;
namespace OrderMicroservice.Core.DTO.OrderDTO;
public record OrderItemAddRequest(string SKU, int Quantity);
