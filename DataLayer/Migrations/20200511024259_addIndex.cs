using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "SponsorTransactions");

            migrationBuilder.DropColumn(
                name: "LateFourNumbersOfBankCard",
                table: "SponsorTransactions");

            migrationBuilder.DropColumn(
                name: "LateFourNumbersOfBankCard",
                table: "BankDatas");

            migrationBuilder.AddColumn<int>(
                name: "LastFourNumbersOfBankCard",
                table: "SponsorTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "color",
                table: "Colleagues",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastFourNumbersOfBankCard",
                table: "BankDatas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFourNumbersOfBankCard",
                table: "SponsorTransactions");

            migrationBuilder.DropColumn(
                name: "LastFourNumbersOfBankCard",
                table: "BankDatas");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "SponsorTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LateFourNumbersOfBankCard",
                table: "SponsorTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "color",
                table: "Colleagues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LateFourNumbersOfBankCard",
                table: "BankDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
