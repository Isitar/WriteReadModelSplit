namespace Application.Category.Commands;

using Domain;
using MediatR;

public record CategoryCreatedEvent(Category Category) : INotification;

public record CreateCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Color { get; set; }
    public string? Description { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
{
    private readonly IDomainContext domainContext;
    private readonly IPublisher publisher;

    public CreateCategoryCommandHandler(IDomainContext domainContext, IPublisher publisher)
    {
        this.domainContext = domainContext;
        this.publisher = publisher;
    }
    
    public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Id, request.Name, request.Description, request.Color);
        domainContext.Categories.Add(category);
        await domainContext.SaveAsync(cancellationToken);
        await publisher.Publish(new CategoryCreatedEvent(category), cancellationToken);
        return Unit.Value;
    }
}