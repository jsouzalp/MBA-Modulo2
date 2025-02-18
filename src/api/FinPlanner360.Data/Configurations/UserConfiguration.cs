using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region Mapping columns

        builder.ToTable("Users");

        builder.HasKey(x => x.UserId)
            .HasName("UsersPK");

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .UseCollation(DatabaseTypeConstant.Collate)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .UseCollation(DatabaseTypeConstant.Collate)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.AuthenticationId)
            .HasColumnName("AuthenticationId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        #endregion Mapping columns

        #region Ignores

        builder.Ignore(x => x.CreatedDate);

        #endregion Ignores

        #region Relationships

        builder.HasMany(x => x.Transactions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("UsersTransactionsFK")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Categories)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("UsersCategoriesFK")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Budgets)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("UsersBudgetsFK")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.GeneralBudgets)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("UsersGeneralBudgetsFK")
            .OnDelete(DeleteBehavior.Cascade);

        #endregion Relationships
    }
}