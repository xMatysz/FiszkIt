using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure;

public class FiszkItDbContext : DbContext
{
    public FiszkItDbContext(DbContextOptions<FiszkItDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FiszkItDbContext).Assembly);
    }
}