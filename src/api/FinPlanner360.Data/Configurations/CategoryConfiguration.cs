using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        #region Mapping columns

        builder.ToTable("Categories");

        builder.HasKey(x => x.CategoryId)
            .HasName("CategoriesPK");

        builder.Property(x => x.CategoryId)
            .HasColumnName("CategoryId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier);

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(25)
            .UseCollation(DatabaseTypeConstant.Collate)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("Type")
            .HasColumnType(DatabaseTypeConstant.Byte)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();
        #endregion Mapping columns

        #region Indexes

        builder.HasIndex(x => x.UserId).HasDatabaseName("CategoriesUserIdIX");

        #endregion Indexes

        #region Relationships

        builder.HasOne(x => x.User)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("CategoriesUserFK")
            .OnDelete(DeleteBehavior.NoAction);

        #endregion Relationships
    }
}