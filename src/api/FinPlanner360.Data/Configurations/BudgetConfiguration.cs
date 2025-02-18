using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Data.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        #region Mapping columns

        builder.ToTable("Budgets");

        builder.HasKey(x => x.BudgetId)
            .HasName("BudgetsPK");

        builder.Property(x => x.BudgetId)
            .HasColumnName("BudgetId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .HasColumnName("CategoryId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnName("Amount")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(2)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();
        #endregion Mapping columns

        #region Indexes

        builder.HasIndex(x => x.UserId).HasDatabaseName("BudgetsUserIdIX");

        #endregion Indexes

        #region Relationships

        builder.HasOne(x => x.User)
            .WithMany(x => x.Budgets)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("BudgetsUserFK")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Budgeties)
            .HasForeignKey(x => x.CategoryId)
            .HasConstraintName("BudgetsCategoryFK")
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}