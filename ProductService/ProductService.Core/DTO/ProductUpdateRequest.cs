﻿namespace ProductService.Core.DTO;
public class ProductUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
