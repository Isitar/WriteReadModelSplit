namespace Infrastructure;

using Application;
using Application.Item;
using Application.Item.Commands;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contract.Item.Vms;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

public class CachedItemReadRepo
    : CachedBaseRepo<ItemVm>,
        IItemReadRepository,
        INotificationHandler<ItemUpdatedEvent>,
        INotificationHandler<ItemCreatedEvent>

{
    private readonly IDomainContext domainContext;
    private readonly IMapper mapper;

    public CachedItemReadRepo(IDistributedCache cache, IDomainContext domainContext, IMapper mapper) : base(cache, mapper)
    {
        this.domainContext = domainContext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ItemListEntryVm>> ItemListEntryVms(IEnumerable<Guid> ids, CancellationToken cancellationToken) => await Vms(ids, cancellationToken);
    public async Task<IEnumerable<ItemVm>> ItemVms(IEnumerable<Guid> ids, CancellationToken cancellationToken) => await Vms(ids, cancellationToken);
  

    public Task Handle(ItemUpdatedEvent notification, CancellationToken cancellationToken) => SetCache(notification.Item, cancellationToken);

    public Task Handle(ItemCreatedEvent notification, CancellationToken cancellationToken) => SetCache(notification.Item, cancellationToken);

    protected override async Task<ItemVm> GetFromDb(Guid id, CancellationToken cancellationToken)
    {
        return await domainContext.Items.Where(i => i.Id == id).ProjectTo<ItemVm>(mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken)
               ?? throw new Exception("Item not found");
    }
}

public class ItemVmProfile : Profile
{
    public ItemVmProfile()
    {
        CreateMap<Item, ItemSimpleVm>();
        CreateMap<Item, ItemListEntryVm>()
            .IncludeBase<Item, ItemSimpleVm>();
        CreateMap<Item, ItemVm>()
            .IncludeBase<Item, ItemListEntryVm>();
    }
}