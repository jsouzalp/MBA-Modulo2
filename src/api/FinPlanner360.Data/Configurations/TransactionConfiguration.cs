using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        #region Mapping columns

        builder.ToTable("Transactions");

        builder.HasKey(x => x.TransactionId)
            .HasName("TransactionsPK");

        builder.Property(x => x.TransactionId)
            .HasColumnName("TransactionId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .UseCollation(DatabaseTypeConstant.Collate)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnName("Amount")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(2)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .HasColumnName("CategoryId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.TransactionDate)
            .HasColumnName("TransactionDate")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();
        #endregion Mapping columns

        #region Indexes

        builder.HasIndex(x => x.UserId).HasDatabaseName("TransactionsUserIdIX");

        #endregion Indexes

        #region Relationships

        builder.HasOne(x => x.User)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("TransactionsUserFK")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.CategoryId)
            .HasConstraintName("TransactionsCategoryFK")
            .OnDelete(DeleteBehavior.Restrict);

        #endregion Relationships
    }
}