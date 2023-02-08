using System;
using Microsoft.EntityFrameworkCore;
using OrderingAPI.Model;

namespace OrderingAPI.Infrastructure;

public class OrderDBContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public OrderDBContext(DbContextOptions<OrderDBContext> options)
        : base(options)
    {
        // No need for change tracking.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }
}

