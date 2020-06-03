using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(maxLength: 50, nullable: false),
                    Owner = table.Column<string>(maxLength: 100, nullable: false),
                    AccountNumber = table.Column<long>(nullable: false),
                    CardNumber = table.Column<long>(nullable: false),
                    ShebaNumber = table.Column<string>(maxLength: 26, nullable: false),
                    pic = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankID);
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
                    color = table.Column<string>(maxLength: 7, nullable: true),
                    picName = table.Column<string>(maxLength: 500, nullable: true),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleagues", x => x.ColleagueID);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptData",
                columns: table => new
                {
                    ReceiptDataID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    ReceiptNumber = table.Column<long>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptData", x => x.ReceiptDataID);
                });

            migrationBuilder.CreateTable(
                name: "BankData",
                columns: table => new
                {
                    BankDataID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankID = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TrackingNumber = table.Column<string>(maxLength: 100, nullable: false),
                    LastFourNumbersOfBankCard = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankData", x => x.BankDataID);
                    table.ForeignKey(
                        name: "FK_BankData_Banks_BankID",
                        column: x => x.BankID,
                        principalTable: "Banks",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Cascade);
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
                    picName = table.Column<string>(maxLength: 500, nullable: true),
                    isActive = table.Column<bool>(nullable: false)
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
                name: "BankTransactions",
                columns: table => new
                {
                    BankTransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionBankDataID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => x.BankTransactionID);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankData_TransactionBankDataID",
                        column: x => x.TransactionBankDataID,
                        principalTable: "BankData",
                        principalColumn: "BankDataID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SponsorTransactions",
                columns: table => new
                {
                    SponsorTransactionsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponsorID = table.Column<int>(nullable: false),
                    ColleagueID = table.Column<int>(nullable: false),
                    CauseOfSupport = table.Column<string>(maxLength: 500, nullable: true),
                    OtherSupport = table.Column<string>(maxLength: 500, nullable: true),
                    MyTransactionBankDataID = table.Column<int>(nullable: true),
                    MyReceiptReceiptDataID = table.Column<int>(nullable: true),
                    isValid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorTransactions", x => x.SponsorTransactionsID);
                    table.ForeignKey(
                        name: "FK_SponsorTransactions_ReceiptData_MyReceiptReceiptDataID",
                        column: x => x.MyReceiptReceiptDataID,
                        principalTable: "ReceiptData",
                        principalColumn: "ReceiptDataID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SponsorTransactions_BankData_MyTransactionBankDataID",
                        column: x => x.MyTransactionBankDataID,
                        principalTable: "BankData",
                        principalColumn: "BankDataID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SponsorTransactions_Sponsors_SponsorID",
                        column: x => x.SponsorID,
                        principalTable: "Sponsors",
                        principalColumn: "SponsorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankData_BankID",
                table: "BankData",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_TransactionBankDataID",
                table: "BankTransactions",
                column: "TransactionBankDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Colleagues_PhoneNumber",
                table: "Colleagues",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_ColleagueID",
                table: "Sponsors",
                column: "ColleagueID");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_PhoneNumber",
                table: "Sponsors",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SponsorTransactions_MyReceiptReceiptDataID",
                table: "SponsorTransactions",
                column: "MyReceiptReceiptDataID");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorTransactions_MyTransactionBankDataID",
                table: "SponsorTransactions",
                column: "MyTransactionBankDataID");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorTransactions_SponsorID",
                table: "SponsorTransactions",
                column: "SponsorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactions");

            migrationBuilder.DropTable(
                name: "SponsorTransactions");

            migrationBuilder.DropTable(
                name: "ReceiptData");

            migrationBuilder.DropTable(
                name: "BankData");

            migrationBuilder.DropTable(
                name: "Sponsors");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Colleagues");
        }
    }
}
