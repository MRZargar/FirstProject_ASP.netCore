using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class ErrorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isValid",
                table: "SponsorTransactions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    ErrorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponsorName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    ReceiptNumber = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    TrackingNumber = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(maxLength: 500, nullable: false),
                    ColleagueID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.ErrorID);
                    table.ForeignKey(
                        name: "FK_Errors_Colleagues_ColleagueID",
                        column: x => x.ColleagueID,
                        principalTable: "Colleagues",
                        principalColumn: "ColleagueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Errors_ColleagueID",
                table: "Errors",
                column: "ColleagueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.AlterColumn<int>(
                name: "isValid",
                table: "SponsorTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
