using System.ComponentModel.DataAnnotations;

namespace ProductMicroservice.Core.DTO;
public class ProductAddRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(45, MinimumLength = 3,ErrorMessage ="Name must be 3-45 characters")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage ="Description is required")]
    [StringLength(150, MinimumLength = 20, ErrorMessage ="The description must be 20-150 characters")]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage ="CategoryId is required")]
    public Guid? CategoryId { get; set; }

    [Required(ErrorMessage = "Manufacturer is required")]
    [StringLength(50,MinimumLength = 3,ErrorMessage = "Manufacturer must be 3-50 characters")]
    public string Manufacturer { get; set; } = default!;
}
