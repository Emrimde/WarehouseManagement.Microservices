using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.DTO;

namespace ProductMicroservice.Core.Mappers;

public static class CategoryMapper
{
    public static CategoryResponse ToCategoryResponse(this Category category)
    {
        return new CategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
        };
    }

    public static Category ToCategory(this CategoryAddRequest request) {

        return new Category()
        {
            Name = request.Name.Trim()
        };
    }

    public static Category ToCategory(this CategoryUpdateRequest request)
    {
        return new Category()
        {
            Name = request.Name,
        };
    }

}
