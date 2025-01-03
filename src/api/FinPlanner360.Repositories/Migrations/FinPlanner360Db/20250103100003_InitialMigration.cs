using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPlanner360.Repositories.Migrations.FinPlanner360Db
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_USER",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    NAME = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false, collation: "Latin1_General_CI_AI"),
                    EMAIL = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false, collation: "Latin1_General_CI_AI"),
                    AUTHENTICATION_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_BUDGET",
                columns: table => new
                {
                    BUDGET_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CATEGORY_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "Money", precision: 2, nullable: false),
                    USER_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DateTime", nullable: false),
                    REMOVED_DATE = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_BUDGET", x => x.BUDGET_ID);
                    table.ForeignKey(
                        name: "FK_TB_BUDGET_01",
                        column: x => x.USER_ID,
                        principalTable: "TB_USER",
                        principalColumn: "USER_ID");
                });

            migrationBuilder.CreateTable(
                name: "TB_CATEGORY",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    USER_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "Varchar", maxLength: 25, nullable: false, collation: "Latin1_General_CI_AI"),
                    TYPE = table.Column<int>(type: "TinyInt", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DateTime", nullable: false),
                    REMOVED_DATE = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CATEGORY", x => x.CATEGORY_ID);
                    table.ForeignKey(
                        name: "FK_TB_CATEGORY_01",
                        column: x => x.USER_ID,
                        principalTable: "TB_USER",
                        principalColumn: "USER_ID");
                });

            migrationBuilder.CreateTable(
                name: "TB_GENERAL_BUDGET",
                columns: table => new
                {
                    GENERAL_BUDGET_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "Money", precision: 2, nullable: true),
                    PERCENTAGE = table.Column<decimal>(type: "Money", precision: 0, nullable: true),
                    USER_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GENERAL_BUDGET", x => x.GENERAL_BUDGET_ID);
                    table.ForeignKey(
                        name: "FK_TB_GENERAL_BUDGET_01",
                        column: x => x.USER_ID,
                        principalTable: "TB_USER",
                        principalColumn: "USER_ID");
                });

            migrationBuilder.CreateTable(
                name: "TB_TRANSACTION",
                columns: table => new
                {
                    TRANSACTION_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "Varchar", maxLength: 50, nullable: false, collation: "Latin1_General_CI_AI"),
                    AMOUNT = table.Column<decimal>(type: "Money", precision: 2, nullable: false),
                    TYPE = table.Column<int>(type: "TinyInt", nullable: false),
                    CATEGORY_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "SmallDateTime", nullable: false),
                    USER_ID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DateTime", nullable: false),
                    REMOVED_DATE = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TRANSACTION", x => x.TRANSACTION_ID);
                    table.ForeignKey(
                        name: "FK_TB_TRANSACTION_01",
                        column: x => x.USER_ID,
                        principalTable: "TB_USER",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_TB_TRANSACTION_02",
                        column: x => x.CATEGORY_ID,
                        principalTable: "TB_CATEGORY",
                        principalColumn: "CATEGORY_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_TB_BUDGET_01",
                table: "TB_BUDGET",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_TB_CATEGORY_01",
                table: "TB_CATEGORY",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_TB_GENERAL_BUDGET_01",
                table: "TB_GENERAL_BUDGET",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_TB_TRANSACTION_01",
                table: "TB_TRANSACTION",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSACTION_CATEGORY_ID",
                table: "TB_TRANSACTION",
                column: "CATEGORY_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_BUDGET");

            migrationBuilder.DropTable(
                name: "TB_GENERAL_BUDGET");

            migrationBuilder.DropTable(
                name: "TB_TRANSACTION");

            migrationBuilder.DropTable(
                name: "TB_CATEGORY");

            migrationBuilder.DropTable(
                name: "TB_USER");
        }
    }
}
