namespace Application.Category.Queries;

using Contract.Category.Vms;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CategoryListQuery : IRequest<IEnumerable<CategoryListEntryVm>>
{
    public string Fulltext { get; set; }
}

public class CategoryListQueryHandler : IRequestHandler<CategoryListQuery, IEnumerable<CategoryListEntryVm>>
{
    private readonly IDomainContext domainContext;
    private readonly ICategoryReadRepository itemReadRepository;

    public CategoryListQueryHandler(IDomainContext domainContext, ICategoryReadRepository itemReadRepository)
    {
        this.domainContext = domainContext;
        this.itemReadRepository = itemReadRepository;
    }

    public async Task<IEnumerable<CategoryListEntryVm>> Handle(CategoryListQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = domainContext.Categories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Fulltext))
        {
            var ft = request.Fulltext.ToLowerInvariant().Trim();
            baseQuery = baseQuery.Where(i => i.Name.ToLower().Contains(ft));
        }

        var ids = await baseQuery.Select(i => i.Id).ToListAsync(cancellationToken);
        var categories = await itemReadRepository.CategoryListEntryVms(ids, cancellationToken);
        return categories;
    }
}