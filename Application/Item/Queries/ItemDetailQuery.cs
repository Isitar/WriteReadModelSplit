namespace Application.Item.Queries;

using Contract.Item.Vms;
using MediatR;

public record ItemDetailQuery : IRequest<ItemVm>
{
    public Guid Id { get; set; }
}

public class ItemDetailQueryHandler : IRequestHandler<ItemDetailQuery, ItemVm>
{
    private readonly IItemReadRepository itemReadRepository;

    public ItemDetailQueryHandler(IItemReadRepository itemReadRepository)
    {
        this.itemReadRepository = itemReadRepository;
    }
    public async Task<ItemVm> Handle(ItemDetailQuery request, CancellationToken cancellationToken)
    {
        return (await itemReadRepository.ItemVms(new[] { request.Id }, cancellationToken)).FirstOrDefault() ?? throw new Exception("Item not found");
    }
}