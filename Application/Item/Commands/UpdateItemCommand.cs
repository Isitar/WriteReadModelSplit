namespace Application.Item.Commands;

using Domain;
using MediatR;

public record ItemUpdatedEvent(Item Item) : INotification;

public record UpdateItemCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly IDomainContext domainContext;
    private readonly IPublisher publisher;

    public UpdateItemCommandHandler(IDomainContext domainContext, IPublisher publisher)
    {
        this.domainContext = domainContext;
        this.publisher = publisher;
    }

    public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = (await domainContext.Items.FindAsync(new object[] { request.Id }, cancellationToken)) ?? throw new Exception("Item not found");
        item.UpdateBasicData(request.Name, request.Price);
        await domainContext.SaveAsync(cancellationToken);
        await publisher.Publish(new ItemUpdatedEvent(item), cancellationToken);
        return Unit.Value;
    }
}