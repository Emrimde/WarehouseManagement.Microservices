using Microsoft.EntityFrameworkCore;
using OrderMicroservice.Core.Domain.Entities;

namespace OrderMicroservice.Infrastructure.DatabaseContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    public ApplicationDbContext() { }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
