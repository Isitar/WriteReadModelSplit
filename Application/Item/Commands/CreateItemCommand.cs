namespace Application.Item.Commands;

using Domain;
using MediatR;

public record ItemCreatedEvent(Item Item) : INotification;

public record CreateItemCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand>
{
    private readonly IDomainContext domainContext;
    private readonly IPublisher publisher;

    public CreateItemCommandHandler(IDomainContext domainContext, IPublisher publisher)
    {
        this.domainContext = domainContext;
        this.publisher = publisher;
    }

    public async Task<Unit> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var category = await domainContext.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken);
        if (category is null)
        {
            throw new Exception("Category not found");
        }
        var item = Item.Create(request.Id, request.Name, request.Price, category);
        domainContext.Items.Add(item);
        await domainContext.SaveAsync(cancellationToken);
        await publisher.Publish(new ItemCreatedEvent(item), cancellationToken);
        return Unit.Value;
    }
}