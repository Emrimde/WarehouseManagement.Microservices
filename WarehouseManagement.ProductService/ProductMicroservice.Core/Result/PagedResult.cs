namespace ProductMicroservice.Core.Result;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = default!;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int? TotalCount { get; set; }
}
