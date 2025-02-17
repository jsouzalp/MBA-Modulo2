using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinPlanner360.Data.Migrations.FinPlanner360Db
{
    /// <inheritdoc />
    public partial class RenameTablesAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_BUDGET_01",
                table: "TB_BUDGET");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_BUDGET_02",
                table: "TB_BUDGET");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_CATEGORY_01",
                table: "TB_CATEGORY");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_GENERAL_BUDGET_01",
                table: "TB_GENERAL_BUDGET");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_TRANSACTION_01",
                table: "TB_TRANSACTION");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_TRANSACTION_02",
                table: "TB_TRANSACTION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_USER",
                table: "TB_USER");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_TRANSACTION",
                table: "TB_TRANSACTION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_GENERAL_BUDGET",
                table: "TB_GENERAL_BUDGET");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_CATEGORY",
                table: "TB_CATEGORY");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_BUDGET",
                table: "TB_BUDGET");

            migrationBuilder.RenameTable(
                name: "TB_USER",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TB_TRANSACTION",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "TB_GENERAL_BUDGET",
                newName: "GeneralBudgets");

            migrationBuilder.RenameTable(
                name: "TB_CATEGORY",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "TB_BUDGET",
                newName: "Budgets");

            migrationBuilder.RenameColumn(
                name: "NAME",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "AUTHENTICATION_ID",
                table: "Users",
                newName: "AuthenticationId");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "DESCRIPTION",
                table: "Transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AMOUNT",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "Transactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TRANSACTION_DATE",
                table: "Transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "CREATED_DATE",
                table: "Transactions",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CATEGORY_ID",
                table: "Transactions",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "TRANSACTION_ID",
                table: "Transactions",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TB_TRANSACTION_CATEGORY_ID",
                table: "Transactions",
                newName: "IX_Transactions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IDX_TB_TRANSACTION_01",
                table: "Transactions",
                newName: "IX_Transactions_UserId");

            migrationBuilder.RenameColumn(
                name: "PERCENTAGE",
                table: "GeneralBudgets",
                newName: "Percentage");

            migrationBuilder.RenameColumn(
                name: "AMOUNT",
                table: "GeneralBudgets",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "GeneralBudgets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "GENERAL_BUDGET_ID",
                table: "GeneralBudgets",
                newName: "GeneralBudgetId");

            migrationBuilder.RenameIndex(
                name: "IDX_TB_GENERAL_BUDGET_01",
                table: "GeneralBudgets",
                newName: "IX_GenerealBudgets_UserId");

            migrationBuilder.RenameColumn(
                name: "TYPE",
                table: "Categories",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "DESCRIPTION",
                table: "Categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "Categories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CREATED_DATE",
                table: "Categories",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CATEGORY_ID",
                table: "Categories",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IDX_TB_CATEGORY_01",
                table: "Categories",
                newName: "IX_Categories_UserId");

            migrationBuilder.RenameColumn(
                name: "AMOUNT",
                table: "Budgets",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "Budgets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CREATED_DATE",
                table: "Budgets",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CATEGORY_ID",
                table: "Budgets",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "BUDGET_ID",
                table: "Budgets",
                newName: "BudgetId");

            migrationBuilder.RenameIndex(
                name: "IX_TB_BUDGET_CATEGORY_ID",
                table: "Budgets",
                newName: "IX_Budgets_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IDX_TB_BUDGET_01",
                table: "Budgets",
                newName: "IX_Budgets_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralBudgets",
                table: "GeneralBudgets",
                column: "GeneralBudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Category",
                table: "Budgets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_User",
                table: "Budgets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_User",
                table: "Categories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenerealBudgets_User",
                table: "GeneralBudgets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Category",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_User",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Category",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_User",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_User",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_GenerealBudgets_User",
                table: "GeneralBudgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Category",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_User",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralBudgets",
                table: "GeneralBudgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "TB_USER");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "TB_TRANSACTION");

            migrationBuilder.RenameTable(
                name: "GeneralBudgets",
                newName: "TB_GENERAL_BUDGET");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "TB_CATEGORY");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "TB_BUDGET");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TB_USER",
                newName: "NAME");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "TB_USER",
                newName: "EMAIL");

            migrationBuilder.RenameColumn(
                name: "AuthenticationId",
                table: "TB_USER",
                newName: "AUTHENTICATION_ID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TB_USER",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TB_TRANSACTION",
                newName: "DESCRIPTION");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "TB_TRANSACTION",
                newName: "AMOUNT");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TB_TRANSACTION",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "TB_TRANSACTION",
                newName: "TRANSACTION_DATE");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TB_TRANSACTION",
                newName: "CREATED_DATE");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "TB_TRANSACTION",
                newName: "CATEGORY_ID");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "TB_TRANSACTION",
                newName: "TRANSACTION_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserId",
                table: "TB_TRANSACTION",
                newName: "IDX_TB_TRANSACTION_01");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CategoryId",
                table: "TB_TRANSACTION",
                newName: "IX_TB_TRANSACTION_CATEGORY_ID");

            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "TB_GENERAL_BUDGET",
                newName: "PERCENTAGE");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "TB_GENERAL_BUDGET",
                newName: "AMOUNT");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TB_GENERAL_BUDGET",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "GeneralBudgetId",
                table: "TB_GENERAL_BUDGET",
                newName: "GENERAL_BUDGET_ID");

            migrationBuilder.RenameIndex(
                name: "IX_GenerealBudgets_UserId",
                table: "TB_GENERAL_BUDGET",
                newName: "IDX_TB_GENERAL_BUDGET_01");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TB_CATEGORY",
                newName: "TYPE");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TB_CATEGORY",
                newName: "DESCRIPTION");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TB_CATEGORY",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TB_CATEGORY",
                newName: "CREATED_DATE");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "TB_CATEGORY",
                newName: "CATEGORY_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserId",
                table: "TB_CATEGORY",
                newName: "IDX_TB_CATEGORY_01");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "TB_BUDGET",
                newName: "AMOUNT");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TB_BUDGET",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TB_BUDGET",
                newName: "CREATED_DATE");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "TB_BUDGET",
                newName: "CATEGORY_ID");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "TB_BUDGET",
                newName: "BUDGET_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_UserId",
                table: "TB_BUDGET",
                newName: "IDX_TB_BUDGET_01");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_CategoryId",
                table: "TB_BUDGET",
                newName: "IX_TB_BUDGET_CATEGORY_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_USER",
                table: "TB_USER",
                column: "USER_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_TRANSACTION",
                table: "TB_TRANSACTION",
                column: "TRANSACTION_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_GENERAL_BUDGET",
                table: "TB_GENERAL_BUDGET",
                column: "GENERAL_BUDGET_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_CATEGORY",
                table: "TB_CATEGORY",
                column: "CATEGORY_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_BUDGET",
                table: "TB_BUDGET",
                column: "BUDGET_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_BUDGET_01",
                table: "TB_BUDGET",
                column: "USER_ID",
                principalTable: "TB_USER",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_BUDGET_02",
                table: "TB_BUDGET",
                column: "CATEGORY_ID",
                principalTable: "TB_CATEGORY",
                principalColumn: "CATEGORY_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_CATEGORY_01",
                table: "TB_CATEGORY",
                column: "USER_ID",
                principalTable: "TB_USER",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_GENERAL_BUDGET_01",
                table: "TB_GENERAL_BUDGET",
                column: "USER_ID",
                principalTable: "TB_USER",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_TRANSACTION_01",
                table: "TB_TRANSACTION",
                column: "USER_ID",
                principalTable: "TB_USER",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_TRANSACTION_02",
                table: "TB_TRANSACTION",
                column: "CATEGORY_ID",
                principalTable: "TB_CATEGORY",
                principalColumn: "CATEGORY_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
