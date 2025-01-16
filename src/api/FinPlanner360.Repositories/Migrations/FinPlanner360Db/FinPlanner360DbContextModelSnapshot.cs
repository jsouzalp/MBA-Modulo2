﻿// <auto-generated />
using System;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinPlanner360.Repositories.Migrations.FinPlanner360Db
{
    [DbContext(typeof(FinPlanner360DbContext))]
    partial class FinPlanner360DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("FinPlanner360.Business.Models.Budget", b =>
                {
                    b.Property<Guid>("BudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("BUDGET_ID");

                    b.Property<decimal>("Amount")
                        .HasPrecision(2)
                        .HasColumnType("Money")
                        .HasColumnName("AMOUNT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("CATEGORY_ID");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("CREATED_DATE");

                    b.Property<Guid>("UserId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("USER_ID");

                    b.HasKey("BudgetId")
                        .HasName("PK_TB_BUDGET");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IDX_TB_BUDGET_01");

                    b.ToTable("TB_BUDGET", (string)null);
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("CATEGORY_ID");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("CREATED_DATE");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("Varchar")
                        .HasColumnName("DESCRIPTION")
                        .UseCollation("Latin1_General_CI_AI");

                    b.Property<int>("Type")
                        .HasColumnType("TinyInt")
                        .HasColumnName("TYPE");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("USER_ID");

                    b.HasKey("CategoryId")
                        .HasName("PK_TB_CATEGORY");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IDX_TB_CATEGORY_01");

                    b.ToTable("TB_CATEGORY", (string)null);
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.GeneralBudget", b =>
                {
                    b.Property<Guid>("GeneralBudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("GENERAL_BUDGET_ID");

                    b.Property<decimal?>("Amount")
                        .HasPrecision(2)
                        .HasColumnType("Money")
                        .HasColumnName("AMOUNT");

                    b.Property<decimal?>("Percentage")
                        .HasPrecision(0)
                        .HasColumnType("Money")
                        .HasColumnName("PERCENTAGE");

                    b.Property<Guid>("UserId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("USER_ID");

                    b.HasKey("GeneralBudgetId")
                        .HasName("PK_TB_GENERAL_BUDGET");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IDX_TB_GENERAL_BUDGET_01");

                    b.ToTable("TB_GENERAL_BUDGET", (string)null);
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.Transaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("TRANSACTION_ID");

                    b.Property<decimal>("Amount")
                        .HasPrecision(2)
                        .HasColumnType("Money")
                        .HasColumnName("AMOUNT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("CATEGORY_ID");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("CREATED_DATE");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("Varchar")
                        .HasColumnName("DESCRIPTION")
                        .UseCollation("Latin1_General_CI_AI");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("SmallDateTime")
                        .HasColumnName("TRANSACTION_DATE");

                    b.Property<Guid>("UserId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("USER_ID");

                    b.HasKey("TransactionId")
                        .HasName("PK_TB_TRANSACTION");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IDX_TB_TRANSACTION_01");

                    b.ToTable("TB_TRANSACTION", (string)null);
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("USER_ID");

                    b.Property<Guid>("AuthenticationId")
                        .HasColumnType("UniqueIdentifier")
                        .HasColumnName("AUTHENTICATION_ID");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("Varchar")
                        .HasColumnName("EMAIL")
                        .UseCollation("Latin1_General_CI_AI");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("Varchar")
                        .HasColumnName("NAME")
                        .UseCollation("Latin1_General_CI_AI");

                    b.HasKey("UserId")
                        .HasName("PK_TB_USER");

                    b.ToTable("TB_USER", (string)null);
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.Budget", b =>
                {
                    b.HasOne("FinPlanner360.Business.Models.Category", "Category")
                        .WithMany("Budgeties")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_TB_BUDGET_02");

                    b.HasOne("FinPlanner360.Business.Models.User", "User")
                        .WithMany("Budgets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_TB_BUDGET_01");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.Category", b =>
                {
                    b.HasOne("FinPlanner360.Business.Models.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_TB_CATEGORY_01");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.GeneralBudget", b =>
                {
                    b.HasOne("FinPlanner360.Business.Models.User", "User")
                        .WithMany("GeneralBudgets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_TB_GENERAL_BUDGET_01");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.Transaction", b =>
                {
                    b.HasOne("FinPlanner360.Business.Models.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_TB_TRANSACTION_02");

                    b.HasOne("FinPlanner360.Business.Models.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_TB_TRANSACTION_01");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.Category", b =>
                {
                    b.Navigation("Budgeties");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("FinPlanner360.Business.Models.User", b =>
                {
                    b.Navigation("Budgets");

                    b.Navigation("Categories");

                    b.Navigation("GeneralBudgets");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
