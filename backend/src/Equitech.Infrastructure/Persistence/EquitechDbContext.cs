using Microsoft.EntityFrameworkCore;
using Equitech.Infrastructure.Persistence.Entities;

namespace Equitech.Infrastructure.Persistence;

public class EquitechDbContext : DbContext
{
    public EquitechDbContext(DbContextOptions<EquitechDbContext> options) : base(options)
    {
    }

    public DbSet<InterviewPackEntity> InterviewPacks => Set<InterviewPackEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InterviewPackEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.JobId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ProfileId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ContentJson).IsRequired();
            entity.HasIndex(e => new { e.UserId, e.JobId });
        });
    }
}
