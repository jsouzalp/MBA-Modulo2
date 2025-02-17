using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPlanner360.Data.Migrations.FinPlanner360Db
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
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
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_User",
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
                    table.PrimaryKey("PK_GeneralBudgets", x => x.GeneralBudgetId);
                    table.ForeignKey(
                        name: "FK_GeneralBudgets_User",
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
                    table.PrimaryKey("PK_Budgets", x => x.BudgetId);
                    table.ForeignKey(
                        name: "FK_Budgets_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Budgets_Category",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
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
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Transactions_Category",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            // Índices adicionais, se necessário
            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CategoryId",
                table: "Budgets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralBudgets_UserId",
                table: "GeneralBudgets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Código para reverter as mudanças no banco de dados, se necessário
            migrationBuilder.DropTable(name: "Transactions");
            migrationBuilder.DropTable(name: "Budgets");
            migrationBuilder.DropTable(name: "GeneralBudgets");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
