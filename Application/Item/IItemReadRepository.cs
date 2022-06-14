namespace Application.Item;

using Contract.Item.Vms;

public interface IItemReadRepository
{
    public Task<IEnumerable<ItemListEntryVm>> ItemListEntryVms(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    public Task<IEnumerable<ItemVm>> ItemVms(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}