using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class debugBankDataTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankData_Banks_BankID",
                table: "BankData");

            migrationBuilder.DropIndex(
                name: "IX_BankData_BankID",
                table: "BankData");

            migrationBuilder.DropColumn(
                name: "BankID",
                table: "BankData");

            migrationBuilder.AddColumn<int>(
                name: "BankID",
                table: "BankTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BankID",
                table: "BankTransactions",
                column: "BankID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_Banks_BankID",
                table: "BankTransactions",
                column: "BankID",
                principalTable: "Banks",
                principalColumn: "BankID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_Banks_BankID",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_BankID",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "BankID",
                table: "BankTransactions");

            migrationBuilder.AddColumn<int>(
                name: "BankID",
                table: "BankData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankData_BankID",
                table: "BankData",
                column: "BankID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankData_Banks_BankID",
                table: "BankData",
                column: "BankID",
                principalTable: "Banks",
                principalColumn: "BankID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
