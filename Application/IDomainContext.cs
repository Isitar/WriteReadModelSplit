namespace Application;

using Microsoft.EntityFrameworkCore;

public interface IDomainContext
{
    public DbSet<Domain.Item> Items { get;  }
    public DbSet<Domain.Category> Categories { get;  }
    public Task SaveAsync(CancellationToken cancellationToken);
}