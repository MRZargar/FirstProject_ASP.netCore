using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class bug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDatas_Bank_BankID",
                table: "BankDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bank",
                table: "Bank");

            migrationBuilder.RenameTable(
                name: "Bank",
                newName: "Banks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banks",
                table: "Banks",
                column: "BankID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankDatas_Banks_BankID",
                table: "BankDatas",
                column: "BankID",
                principalTable: "Banks",
                principalColumn: "BankID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDatas_Banks_BankID",
                table: "BankDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banks",
                table: "Banks");

            migrationBuilder.RenameTable(
                name: "Banks",
                newName: "Bank");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bank",
                table: "Bank",
                column: "BankID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankDatas_Bank_BankID",
                table: "BankDatas",
                column: "BankID",
                principalTable: "Bank",
                principalColumn: "BankID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
