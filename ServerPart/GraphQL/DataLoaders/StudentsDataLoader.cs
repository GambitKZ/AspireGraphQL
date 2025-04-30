using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ServerPart.Data;
using ServerPart.Models;

namespace ServerPart.GraphQL.DataLoaders;

public class StudentsDataLoader : DataLoaderBase<int, IEnumerable<Student>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

    private const string Cache_Prefix = "Student_";

    public StudentsDataLoader(AppDbContext dbContext, IMemoryCache cache) : base(false)
    {
        _dbContext = dbContext;
        _cache = cache;

        _memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            // specify a maximum lifetime of 5 minutes
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            // set so that the size of the cache can be limited
            //Size = 1,
        };
    }

    protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<Student>>> list, CancellationToken ctr)
    {
        var unMatched = new List<DataLoaderPair<int, IEnumerable<Student>>>(list.Count());

        // Resolve that we have in cache
        foreach (var entry in list)
        {
            if (_cache.TryGetValue(Cache_Prefix + entry.Key, out var students))
            {
                entry.SetResult((IEnumerable<Student>) students);
            }
            else
            {
                unMatched.Add(entry);
            }
        }

        if (unMatched.Count == 0)
        {
            return;
        }

        // resolve those that are not in cache
        IEnumerable<int> ids = unMatched.Select(pair => pair.Key);
        IDictionary<int, IEnumerable<Student>> data = await _dbContext.Students
            .Where(t => ids.Contains(t.CoursePlanId))
            .GroupBy(s => s.CoursePlanId)
            .ToDictionaryAsync(g => g.Key, g => g.AsEnumerable());

        foreach (var entry in unMatched)
        {
            if (data.TryGetValue(entry.Key, out var students))
            {
                _cache.Set(Cache_Prefix + entry.Key, students, _memoryCacheEntryOptions);
                entry.SetResult(students);
            }
            else
            {
                // Do I need this part?
                entry.SetResult(null);
            }
        }
    }

}
