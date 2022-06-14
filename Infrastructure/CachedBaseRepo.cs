namespace Infrastructure;

using System.Text.Json;
using AutoMapper;
using Contract.Common;
using Microsoft.Extensions.Caching.Distributed;

public abstract class CachedBaseRepo<TVm> where TVm : IEntity
{
    private readonly IDistributedCache cache;
    private readonly IMapper mapper;

    public CachedBaseRepo(IDistributedCache cache, IMapper mapper)
    {
        this.cache = cache;
        this.mapper = mapper;
    }


    protected abstract Task<TVm> GetFromDb(Guid id, CancellationToken cancellationToken);

    protected async Task<TVm> GetFromCache(Guid id, CancellationToken cancellationToken)
    {
        var cachedJson = await cache.GetStringAsync(id.ToString(), cancellationToken);
        if (cachedJson is not null)
        {
            return JsonSerializer.Deserialize<TVm>(cachedJson)!;
        }

        var mappedCategory = await GetFromDb(id, cancellationToken);
        cachedJson = JsonSerializer.Serialize(mappedCategory);
        await cache.SetStringAsync(id.ToString(), cachedJson, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(3) }, cancellationToken);
        return mappedCategory;
    }

    protected async Task<IEnumerable<TVm>> Vms(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var idList = ids.ToArray();
        var retVal = new List<TVm>(idList.Length);
        foreach (var id in idList)
        {
            retVal.Add(await GetFromCache(id, cancellationToken));
        }

        return retVal;
    }

    protected Task SetCache<TDomain>(TDomain d, CancellationToken cancellationToken) => SetCache(mapper.Map<TVm>(d), cancellationToken);
    protected async Task SetCache(TVm vm, CancellationToken cancellationToken)
    {
        await cache.SetStringAsync(vm.Id.ToString(), JsonSerializer.Serialize(vm), new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(3)
        }, cancellationToken);
    }
}