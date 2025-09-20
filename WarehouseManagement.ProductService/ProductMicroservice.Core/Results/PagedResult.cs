namespace ProductMicroservice.Core.Results;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = default!;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int? TotalCount { get; set; }

    public static PagedResult<T> Create(IEnumerable<T> Items, int Page, int PageSize)
    {
        return new PagedResult<T> { Items = Items, Page = Page, PageSize = PageSize };
    }
}
