using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class bug2sheba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShebaNumber",
                table: "Banks",
                maxLength: 26,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ShebaNumber",
                table: "Banks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 26);
        }
    }
}
