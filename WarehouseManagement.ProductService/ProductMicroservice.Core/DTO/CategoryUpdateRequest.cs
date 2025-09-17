namespace ProductMicroservice.Core.DTO;

public class CategoryUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
