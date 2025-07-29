using System.ComponentModel.DataAnnotations;

namespace ProductService.Core.DTO;
public class ProductAddRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string StockKeepingUnit { get; set; } = default!;

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    [StringLength(50)]
    public string Manufacturer { get; set; } = default!;
}
