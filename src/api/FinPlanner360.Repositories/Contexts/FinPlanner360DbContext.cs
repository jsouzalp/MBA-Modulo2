using FinPlanner360.Entities.Budgets;
using FinPlanner360.Entities.Categories;
using FinPlanner360.Entities.GeneralBudgets;
using FinPlanner360.Entities.Transactions;
using FinPlanner360.Entities.Users;
using FinPlanner360.Repositories.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Contexts
{
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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetConfiguration());
            modelBuilder.ApplyConfiguration(new GeneralBudgetConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
