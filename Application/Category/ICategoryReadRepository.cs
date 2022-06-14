namespace Application.Category;

using Contract.Category.Vms;

public interface ICategoryReadRepository
{
    public Task<IEnumerable<CategoryListEntryVm>> CategoryListEntryVms(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    public Task<IEnumerable<CategoryVm>> CategoryVms(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}