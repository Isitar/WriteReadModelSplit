namespace Api.Controllers;

using Application.Category.Commands;
using Application.Category.Queries;
using Application.Item.Commands;
using Application.Item.Queries;
using AutoMapper;
using Contract.Category;
using Contract.Category.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class CategoryController : Controller
{
    [HttpGet(CategoryRoutes.ListCategories)]
    public async Task<IActionResult> ListCategories([FromServices] IMediator mediator)
    {
        var res = await mediator.Send(new CategoryListQuery
        { });

        return Ok(res);
    }
    [HttpGet(CategoryRoutes.CategoryItems)]
    public async Task<IActionResult> CategoryItems([FromServices] IMediator mediator, Guid id)
    {
        var res = await mediator.Send(new ItemListQuery { CategoryIds = new[] { id } });
        return Ok(res);
    }


    [HttpGet(CategoryRoutes.CategoryDetail)]
    public async Task<IActionResult> CategoryDetail([FromServices] IMediator mediator, Guid id)
    {
        var res = await mediator.Send(new CategoryDetailQuery { Id = id });
        return Ok(res);
    }

    [HttpPut(CategoryRoutes.UpdateCategory)]
    public async Task<IActionResult> UpdateCategory([FromServices] IMediator mediator, [FromServices] IMapper mapper, Guid id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
    {
        var res = await mediator.Send(mapper.Map<UpdateCategoryCommand>(updateCategoryRequest) with { Id = id });
        return Ok(res);
    }
    [HttpPost(CategoryRoutes.CreateCategory)]
    public async Task<IActionResult> CreateCategory([FromServices] IMediator mediator, [FromServices] IMapper mapper, [FromBody] CreateCategoryRequest createCategoryCommand)
    {
        var res = await mediator.Send(mapper.Map<CreateCategoryCommand>(createCategoryCommand) with { Id = Guid.NewGuid() });
        return Ok();
    }
    [HttpPost(CategoryRoutes.CreateItem)]
    public async Task<IActionResult> CreateCategory([FromServices] IMediator mediator, [FromServices] IMapper mapper, Guid id, [FromBody] CreateItemRequest createItemRequest)
    {
        var res = await mediator.Send(mapper.Map<CreateItemCommand>(createItemRequest) with { Id = Guid.NewGuid(), CategoryId = id });
        return Ok();
    }
}

public class CategoryProfiles : Profile
{
    public CategoryProfiles()
    {
        CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>()
            .ForMember(vm => vm.Id, opts => opts.Ignore());
        CreateMap<CreateCategoryRequest, CreateCategoryCommand>()
            .ForMember(vm => vm.Id, opts => opts.Ignore());
        CreateMap<CreateItemRequest, CreateItemCommand>()
            .ForMember(vm => vm.Id, opts => opts.Ignore())
            .ForMember(vm => vm.CategoryId, opts => opts.Ignore());
    }
}