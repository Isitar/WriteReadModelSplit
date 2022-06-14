namespace Infrastructure;

using Application;
using Application.Category;
using Application.Category.Commands;
using Application.Item.Commands;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contract.Category.Vms;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

public class CachedCategoryReadRepo
    : CachedBaseRepo<CategoryVm>,
        ICategoryReadRepository,
        INotificationHandler<CategoryCreatedEvent>,
        INotificationHandler<CategoryUpdatedEvent>,
        INotificationHandler<ItemUpdatedEvent>,
        INotificationHandler<ItemCreatedEvent>

{
    private readonly IDomainContext domainContext;
    private readonly IMapper mapper;

    public CachedCategoryReadRepo(IDistributedCache cache, IDomainContext domainContext, IMapper mapper) : base(cache, mapper)
    {
        this.domainContext = domainContext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<CategoryListEntryVm>> CategoryListEntryVms(IEnumerable<Guid> ids, CancellationToken cancellationToken) => await Vms(ids, cancellationToken);

    public async Task<IEnumerable<CategoryVm>> CategoryVms(IEnumerable<Guid> ids, CancellationToken cancellationToken) => await Vms(ids, cancellationToken);


    public Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken) => SetCache(notification.Category, cancellationToken);

    public Task Handle(CategoryUpdatedEvent notification, CancellationToken cancellationToken) => SetCache(notification.Category, cancellationToken);

    public Task Handle(ItemUpdatedEvent notification, CancellationToken cancellationToken) => SetCache(notification.Item.Category, cancellationToken);

    public Task Handle(ItemCreatedEvent notification, CancellationToken cancellationToken) => SetCache(notification.Item.Category, cancellationToken);

    protected override async Task<CategoryVm> GetFromDb(Guid id, CancellationToken cancellationToken)
    {
        return await domainContext.Categories.Where(c => c.Id == id).ProjectTo<CategoryVm>(mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken)
               ?? throw new Exception("Category not found");
    }
}

public class CategoryVmProfile : Profile
{
    public CategoryVmProfile()
    {
        CreateMap<Category, CategorySimpleVm>();
        CreateMap<Category, CategoryListEntryVm>()
            .IncludeBase<Category, CategorySimpleVm>();
        CreateMap<Category, CategoryVm>()
            .IncludeBase<Category, CategoryListEntryVm>();
    }
}