using Microsoft.EntityFrameworkCore;
using PickingMicroservice.Core.Domain.Entities;

namespace PickingMicroservice.Infrastructure.DatabaseContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public ApplicationDbContext()
    {
    }
    public DbSet<PickingTask> PickingTasks { get; set; }
}
