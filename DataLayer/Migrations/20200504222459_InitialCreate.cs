using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankDatas",
                columns: table => new
                {
                    BankDataID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(maxLength: 50, nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TrackingNumber = table.Column<string>(maxLength: 100, nullable: false),
                    LateFourNumbersOfBankCard = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDatas", x => x.BankDataID);
                });

            migrationBuilder.CreateTable(
                name: "Colleagues",
                columns: table => new
                {
                    ColleagueID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isMale = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<long>(nullable: false),
                    BirthDay = table.Column<DateTime>(nullable: false),
                    StartActivity = table.Column<DateTime>(nullable: false),
                    code = table.Column<int>(nullable: false),
                    color = table.Column<string>(nullable: true),
                    picName = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleagues", x => x.ColleagueID);
                });

            migrationBuilder.CreateTable(
                name: "Sponsors",
                columns: table => new
                {
                    SponsorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isMale = table.Column<bool>(nullable: false),
                    ColleagueID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<long>(nullable: false),
                    CauseOfSupport = table.Column<string>(maxLength: 500, nullable: true),
                    OtherSupport = table.Column<string>(maxLength: 500, nullable: true),
                    picName = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsors", x => x.SponsorID);
                    table.ForeignKey(
                        name: "FK_Sponsors_Colleagues_ColleagueID",
                        column: x => x.ColleagueID,
                        principalTable: "Colleagues",
                        principalColumn: "ColleagueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SponsorTransactions",
                columns: table => new
                {
                    SponsorTransactionsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponsorID = table.Column<int>(nullable: false),
                    BankName = table.Column<string>(maxLength: 50, nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TrackingNumber = table.Column<string>(nullable: false),
                    LateFourNumbersOfBankCard = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    isValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorTransactions", x => x.SponsorTransactionsID);
                    table.ForeignKey(
                        name: "FK_SponsorTransactions_Sponsors_SponsorID",
                        column: x => x.SponsorID,
                        principalTable: "Sponsors",
                        principalColumn: "SponsorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_ColleagueID",
                table: "Sponsors",
                column: "ColleagueID");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorTransactions_SponsorID",
                table: "SponsorTransactions",
                column: "SponsorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDatas");

            migrationBuilder.DropTable(
                name: "SponsorTransactions");

            migrationBuilder.DropTable(
                name: "Sponsors");

            migrationBuilder.DropTable(
                name: "Colleagues");
        }
    }
}
