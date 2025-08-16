using System.Collections;

namespace OrderMicroservice.Core.DTO;
public record OrderItemAddRequest(string SKU, int Quantity);
