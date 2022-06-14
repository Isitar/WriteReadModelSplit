using Api.Controllers;
using Application;
using Application.Category;
using Application.Item;
using Application.Item.Commands;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.
var services = builder.Services;
services.AddControllers();

services.AddMediatR(typeof(CreateItemCommand).Assembly, typeof(CachedItemReadRepo).Assembly);
services.AddAutoMapper(typeof(ItemProfiles).Assembly, typeof(ItemVmProfile).Assembly);
services.AddDbContext<DomainContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("DomainContext")));
services.AddScoped<IDomainContext, DomainContext>();

services.AddScoped<IItemReadRepository, CachedItemReadRepo>();
services.AddScoped<ICategoryReadRepository, CachedCategoryReadRepo>();


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDistributedMemoryCache();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var domainContext = scope.ServiceProvider.GetRequiredService<DomainContext>();
    domainContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();