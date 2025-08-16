using InventoryMicroservice.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryMicroservice.Infrastructure.DatabaseContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options){}

    public ApplicationDbContext() { }

    public DbSet<InventoryItem> InventoryItems { get; set; }
    
}
