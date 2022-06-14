namespace Infrastructure;

using Application;
using Domain;
using Microsoft.EntityFrameworkCore;

public class DomainContext : DbContext, IDomainContext
{
    public DomainContext(DbContextOptions<DomainContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Category> Categories => Set<Category>();

    public Task SaveAsync(CancellationToken cancellationToken) => SaveChangesAsync(cancellationToken);
}