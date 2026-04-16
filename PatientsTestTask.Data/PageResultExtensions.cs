using Microsoft.EntityFrameworkCore;
using PatientsTestTask.Core;

namespace PatientsTestTask.Data;

internal static class PageResultExtensions
{
    internal static async Task<PageResult<T>> ToPageResult<T>(this IQueryable<T> query, int page, int pageSize)
    {
        var total = await query.CountAsync();
        var result = await query.Skip(page * pageSize).Take(pageSize).ToListAsync();
        return new PageResult<T>
        {
            Values = result,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }
}