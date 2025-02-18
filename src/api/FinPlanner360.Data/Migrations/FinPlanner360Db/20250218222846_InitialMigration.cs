using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPlanner360.Data.Migrations.FinPlanner360Db
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Name = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false, collation: "Latin1_General_CI_AI"),
                    Email = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false, collation: "Latin1_General_CI_AI"),
                    AuthenticationId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UsersPK", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    Description = table.Column<string>(type: "Varchar", maxLength: 25, nullable: false, collation: "Latin1_General_CI_AI"),
                    Type = table.Column<int>(type: "TinyInt", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CategoriesPK", x => x.CategoryId);
                    table.ForeignKey(
                        name: "CategoriesUserFK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GeneralBudgets",
                columns: table => new
                {
                    GeneralBudgetId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "Money", precision: 2, nullable: true),
                    Percentage = table.Column<decimal>(type: "Money", precision: 0, nullable: true),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GeneralBudgetsPK", x => x.GeneralBudgetId);
                    table.ForeignKey(
                        name: "GenerealBudgetsUserFK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "Money", precision: 2, nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BudgetsPK", x => x.BudgetId);
                    table.ForeignKey(
                        name: "BudgetsCategoryFK",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "BudgetsUserFK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Description = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false, collation: "Latin1_General_CI_AI"),
                    Amount = table.Column<decimal>(type: "Money", precision: 2, nullable: false),
                    CategoryId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TransactionsPK", x => x.TransactionId);
                    table.ForeignKey(
                        name: "TransactionsCategoryFK",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "TransactionsUserFK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "BudgetsUserIdIX",
                table: "Budgets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CategoryId",
                table: "Budgets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "CategoriesUserIdIX",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "GenerealBudgetsUserIdIX",
                table: "GeneralBudgets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "TransactionsUserIdIX",
                table: "Transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "GeneralBudgets");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
