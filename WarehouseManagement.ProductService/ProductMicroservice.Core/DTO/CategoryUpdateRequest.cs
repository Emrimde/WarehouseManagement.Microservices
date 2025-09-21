using System.ComponentModel.DataAnnotations;

namespace ProductMicroservice.Core.DTO;

public class CategoryUpdateRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be 3-30 characters")]
    public string Name { get; set; } = default!;
}
