namespace Application.Item.Queries;

using Contract.Item.Vms;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record ItemListQuery : IRequest<IEnumerable<ItemListEntryVm>>
{
    public IEnumerable<Guid> CategoryIds { get; set; } = Array.Empty<Guid>();
    public string? Fulltext { get; set; }
}

public class ItemListQueryHandler : IRequestHandler<ItemListQuery, IEnumerable<ItemListEntryVm>>
{
    private readonly IDomainContext domainContext;
    private readonly IItemReadRepository itemReadRepository;

    public ItemListQueryHandler(IDomainContext domainContext, IItemReadRepository itemReadRepository)
    {
        this.domainContext = domainContext;
        this.itemReadRepository = itemReadRepository;
    }

    public async Task<IEnumerable<ItemListEntryVm>> Handle(ItemListQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = domainContext.Items.AsQueryable();
        if (request.CategoryIds.Any())
        {
            baseQuery = baseQuery.Where(i => request.CategoryIds.Contains(i.Category.Id));
        }

        if (!string.IsNullOrWhiteSpace(request.Fulltext))
        {
            var ft = request.Fulltext.ToLowerInvariant().Trim();
            baseQuery = baseQuery.Where(i => i.Name.ToLower().Contains(ft));
        }

        var ids = await baseQuery.Select(i => i.Id).ToListAsync(cancellationToken);
        var items = await itemReadRepository.ItemListEntryVms(ids, cancellationToken);
        return items;
    }
}