using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
using ServerPart.Data;
using ServerPart.Models;

namespace ServerPart.GraphQL.DataLoaders;

public class TeacherDataLoader : DataLoaderBase<Guid, Teacher>
{
    private readonly AppDbContext _dbContext;

    public TeacherDataLoader(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task FetchAsync(IEnumerable<DataLoaderPair<Guid, Teacher>> list, CancellationToken ctr)
    {
        IEnumerable<Guid> ids = list.Select(pair => pair.Key);
        IDictionary<Guid, Teacher> data = await _dbContext.Teachers.Where(t => ids.Contains(t.Id)).ToDictionaryAsync(x => x.Id, ctr);

        foreach (DataLoaderPair<Guid, Teacher> entry in list)
        {
            entry.SetResult(data.TryGetValue(entry.Key, out var teacher) ? teacher : null);
        }

    }
}
