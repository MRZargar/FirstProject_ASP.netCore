using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addIndexAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SponsorTransactions_TransactionDate",
                table: "SponsorTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_PhoneNumber",
                table: "Sponsors",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colleagues_PhoneNumber",
                table: "Colleagues",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankDatas_TransactionDate",
                table: "BankDatas",
                column: "TransactionDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SponsorTransactions_TransactionDate",
                table: "SponsorTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Sponsors_PhoneNumber",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Colleagues_PhoneNumber",
                table: "Colleagues");

            migrationBuilder.DropIndex(
                name: "IX_BankDatas_TransactionDate",
                table: "BankDatas");
        }
    }
}
