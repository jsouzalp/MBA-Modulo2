using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Busines.Models;

namespace FinPlanner360.Repositories.Configurations
{
    public class GeneralBudgetConfiguration : IEntityTypeConfiguration<GeneralBudget>
    {
        public void Configure(EntityTypeBuilder<GeneralBudget> builder)
        {
            #region Mapping columns
            builder.ToTable("TB_GENERAL_BUDGET");

            builder.HasKey(x => x.GeneralBudgetId)
                .HasName("PK_TB_GENERAL_BUDGET");

            builder.Property(x => x.GeneralBudgetId)
                .HasColumnName("GENERAL_BUDGET_ID")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("USER_ID")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("AMOUNT")
                .HasColumnType(DatabaseTypeConstant.Money)
                .HasPrecision(2);

            builder.Property(x => x.Percentage)
                .HasColumnName("PERCENTAGE")
                .HasColumnType(DatabaseTypeConstant.Money)
                .HasPrecision(0);

            //builder.Property(x => x.CreatedDate)
            //    .HasColumnName("CREATED_DATE")
            //    .HasColumnType(DatabaseTypeConstant.DateTime)
            //    .IsRequired();

            //builder.Property(x => x.RemovedDate)
            //    .HasColumnName("REMOVED_DATE")
            //    .HasColumnType(DatabaseTypeConstant.DateTime);
            #endregion

            #region Ignores
            builder.Ignore(x => x.CreatedDate);
            builder.Ignore(x => x.RemovedDate);
            #endregion

            #region Indexes
            builder.HasIndex(x => x.UserId).HasDatabaseName("IDX_TB_GENERAL_BUDGET_01");
            #endregion

            #region Relationships
            builder.HasOne(x => x.User)
                .WithMany(x => x.GeneralBudgets)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_TB_GENERAL_BUDGET_01")
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}