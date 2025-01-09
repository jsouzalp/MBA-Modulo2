using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Repositories.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        #region Mapping columns

        builder.ToTable("TB_BUDGET");

        builder.HasKey(x => x.BudgetId)
            .HasName("PK_TB_BUDGET");

        builder.Property(x => x.BudgetId)
            .HasColumnName("BUDGET_ID")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("USER_ID")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .HasColumnName("CATEGORY_ID")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnName("AMOUNT")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(2)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CREATED_DATE")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();
        #endregion Mapping columns

        #region Indexes

        builder.HasIndex(x => x.UserId).HasDatabaseName("IDX_TB_BUDGET_01");

        #endregion Indexes

        #region Relationships

        builder.HasOne(x => x.User)
            .WithMany(x => x.Budgets)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("FK_TB_BUDGET_01")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Budgeties)
            .HasForeignKey(x => x.CategoryId)
            .HasConstraintName("FK_TB_BUDGET_02")
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}