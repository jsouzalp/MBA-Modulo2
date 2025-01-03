using FinPlanner360.Business.Models;
using FinPlanner360.Repositories.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Contexts;

public class FinPlanner360DbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<GeneralBudget> GeneralBudgets { get; set; }

    public FinPlanner360DbContext(DbContextOptions<FinPlanner360DbContext> options)
        : base(options)
    {
        //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        //ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetConfiguration());
        modelBuilder.ApplyConfiguration(new GeneralBudgetConfiguration());

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                   .SelectMany(e => e.GetProperties()
                                                   .Where(p => p.ClrType == typeof(string))))
        {
            if (property.GetMaxLength() == null)
            {
                property.SetMaxLength(100);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}