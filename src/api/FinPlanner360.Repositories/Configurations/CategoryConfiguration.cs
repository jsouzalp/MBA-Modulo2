using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinPlanner360.Busines.Models;

namespace FinPlanner360.Repositories.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            #region Mapping columns
            builder.ToTable("TB_CATEGORY");

            builder.HasKey(x => x.CategoryId)
                .HasName("PK_TB_CATEGORY");

            builder.Property(x => x.CategoryId)
                .HasColumnName("CATEGORY_ID")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("USER_ID")
                .HasColumnType(DatabaseTypeConstant.UniqueIdentifier);

            builder.Property(x => x.Description)
                .HasColumnName("DESCRIPTION")
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasColumnName("TYPE")
                .HasColumnType(DatabaseTypeConstant.Byte)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnName("CREATED_DATE")
                .HasColumnType(DatabaseTypeConstant.DateTime)
                .IsRequired();

            builder.Property(x => x.RemovedDate)
                .HasColumnName("REMOVED_DATE")
                .HasColumnType(DatabaseTypeConstant.DateTime);
            #endregion

            #region Ignores
            #endregion

            #region Indexes
            builder.HasIndex(x => x.UserId).HasDatabaseName("IDX_TB_CATEGORY_01");
            #endregion
            
            #region Relationships
            builder.HasOne(x => x.User)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_TB_CATEGORY_01")
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}