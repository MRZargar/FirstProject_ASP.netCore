using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class changeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isValid",
                table: "Sponsors");

            migrationBuilder.AddColumn<bool>(
                name: "isValid",
                table: "SponsorTransactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMale",
                table: "Sponsors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "picName",
                table: "Sponsors",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "Colleagues",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isMale",
                table: "Colleagues",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "picName",
                table: "Colleagues",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isValid",
                table: "SponsorTransactions");

            migrationBuilder.DropColumn(
                name: "isMale",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "picName",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "color",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "isMale",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "picName",
                table: "Colleagues");

            migrationBuilder.AddColumn<bool>(
                name: "isValid",
                table: "Sponsors",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
