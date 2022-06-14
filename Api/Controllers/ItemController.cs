namespace Api.Controllers;

using Application.Item.Commands;
using Application.Item.Queries;
using AutoMapper;
using Contract.Item;
using Contract.Item.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class ItemController : Controller
{
    [HttpGet(ItemRoutes.ListItems)]
    public async Task<IActionResult> ListItems([FromServices] IMediator mediator, [FromQuery] string? fullText = null)
    {
        var res = await mediator.Send(new ItemListQuery
        {
            Fulltext = fullText
        });

        return Ok(res);
    }

    [HttpGet(ItemRoutes.ItemDetail)]
    public async Task<IActionResult> ItemDetail([FromServices] IMediator mediator, Guid id)
    {
        var res = await mediator.Send(new ItemDetailQuery { Id = id });
        return Ok(res);
    }

    [HttpPut(ItemRoutes.UpdateItem)]
    public async Task<IActionResult> UpdateItem([FromServices] IMediator mediator, [FromServices] IMapper mapper, Guid id, [FromBody] UpdateItemRequest updateItemRequest)
    {
        var res = await mediator.Send(mapper.Map<UpdateItemCommand>(updateItemRequest) with { Id = id });
        return Ok(res);
    }
}

public class ItemProfiles : Profile
{
    public ItemProfiles()
    {
        CreateMap<UpdateItemRequest, UpdateItemCommand>()
            .ForMember(vm => vm.Id, opts => opts.Ignore());
    }
}