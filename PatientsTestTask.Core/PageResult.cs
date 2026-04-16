namespace PatientsTestTask.Core;

public class PageResult<TValue>
{
    public required List<TValue> Values { get; set; }
    public required long Total { get; set; }

    public required int Page { get; set; }
    public required int PageSize { get; set; }
}
