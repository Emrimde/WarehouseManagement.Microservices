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
    public DbSet<PickTask> PickTasks { get; set; }
    public DbSet<PickItem> PickItem { get; set; }
}
