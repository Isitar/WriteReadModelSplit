namespace Application.Category.Queries;

using Contract.Category.Vms;
using MediatR;

public class CategoryDetailQuery : IRequest<CategoryVm>
{
    public Guid Id { get; set; }
}

public class CategoryDetailQueryHandler : IRequestHandler<CategoryDetailQuery, CategoryVm>
{
    private readonly ICategoryReadRepository categoryReadRepository;

    public CategoryDetailQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        this.categoryReadRepository = categoryReadRepository;
    }
    public async Task<CategoryVm> Handle(CategoryDetailQuery request, CancellationToken cancellationToken)
    {
        return (await categoryReadRepository.CategoryVms(new[] { request.Id }, cancellationToken)).FirstOrDefault() ?? throw new Exception("Category not found");
    }
}