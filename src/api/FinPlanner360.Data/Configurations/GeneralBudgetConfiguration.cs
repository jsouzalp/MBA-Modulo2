using FinPlanner360.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinPlanner360.Data.Configurations;

public class GeneralBudgetConfiguration : IEntityTypeConfiguration<GeneralBudget>
{
    public void Configure(EntityTypeBuilder<GeneralBudget> builder)
    {
        #region Mapping columns

        builder.ToTable("GeneralBudgets");

        builder.HasKey(x => x.GeneralBudgetId)
            .HasName("PK_GeneralBudgets");

        builder.Property(x => x.GeneralBudgetId)
            .HasColumnName("GeneralBudgetId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnName("Amount")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(2);

        builder.Property(x => x.Percentage)
            .HasColumnName("Percentage")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(0);

        #endregion Mapping columns

        #region Ignores

        builder.Ignore(x => x.CreatedDate);

        #endregion Ignores

        #region Indexes

        builder.HasIndex(x => x.UserId).HasDatabaseName("IX_GenerealBudgets_UserId");

        #endregion Indexes

        #region Relationships

        builder.HasOne(x => x.User)
            .WithMany(x => x.GeneralBudgets)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("FK_GenerealBudgets_User")
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}