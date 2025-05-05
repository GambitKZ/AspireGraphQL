using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ServerPart.HotChocolate.GraphQL.DataLoaders;

/// <summary>
/// Default "DataLoader" cache the response within one user request (scoped)
/// </summary>
public class StudentsDataLoader : BatchDataLoader<int, Student[]>
{
    private readonly IServiceProvider _services;
    private const string Cache_Prefix = "Student_";

    public StudentsDataLoader(
        IServiceProvider services,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options)
    : base(batchScheduler, options)
    {
        _services = services;
    }

    protected override async Task<IReadOnlyDictionary<int, Student[]>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
        await using var scope = _services.CreateAsyncScope();
        var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

        var cachedDictionary = new Dictionary<int, Student[]>();
        var uncached = new List<int>();

        foreach (var key in keys)
        {
            var cachedResult = memoryCache.Get<Student[]?>(Cache_Prefix + key);
            if (cachedResult != null)
            {
                cachedDictionary.Add(key, cachedResult);
            }
            else
            {
                uncached.Add(key);
            }
        }

        if (uncached.Count == 0)
        {
            return cachedDictionary;
        }

        await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var dbResult = await dbContext.Students
            .Where(s => uncached.Contains(s.CoursePlanId))
            .GroupBy(s => s.CoursePlanId)
            .Select(se => new { se.Key, Items = se.OrderBy(s => s.Name).ToArray() })
            .ToDictionaryAsync(d => d.Key, d => d.Items, cancellationToken);

        foreach (var item in dbResult)
        {
            memoryCache.Set(Cache_Prefix + item.Key, item.Value);
        }

        return cachedDictionary.Concat(dbResult).ToDictionary();
    }
}
