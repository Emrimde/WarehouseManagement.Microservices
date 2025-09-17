using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Core.Domain.Entities;

namespace ProductMicroservice.Infrastructure.DatabaseContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
    public ApplicationDbContext() { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
