using Microsoft.EntityFrameworkCore;
using ProductService.Core.Domain.Entities;

namespace ProductService.Infrastructure.DatabaseContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
    public ApplicationDbContext() { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
