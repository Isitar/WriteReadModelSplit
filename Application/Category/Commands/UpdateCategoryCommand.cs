namespace Application.Category.Commands;

using Domain;
using MediatR;

public record CategoryUpdatedEvent(Category Category) : INotification;
public record UpdateCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Color { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IDomainContext domainContext;
    private readonly IPublisher publisher;

    public UpdateCategoryCommandHandler(IDomainContext domainContext, IPublisher publisher)
    {
        this.domainContext = domainContext;
        this.publisher = publisher;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = (await domainContext.Categories.FindAsync(new object[] { request.Id }, cancellationToken)) ?? throw new Exception("Category not found");
        category.UpdateBasicData(request.Name, request.Description, request.Color);
        await domainContext.SaveAsync(cancellationToken);
        await publisher.Publish(new CategoryUpdatedEvent(category), cancellationToken);
        
        return Unit.Value;
    }
}