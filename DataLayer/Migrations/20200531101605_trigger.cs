using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class trigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER [dbo].[validation]
					ON  [Mehr].[dbo].[SponsorTransactions]
					AFTER INSERT
				AS 
				BEGIN
					declare @check int
					declare @count int

					declare @new_id int
					declare @new_sponsorid int
					declare @new_trackingNumber varchar
					declare @new_amount decimal(18,2)
					declare @new_4number int
					declare @new_date date

					select @new_id = t.SponsorTransactionsID,
							@new_sponsorid = t.SponsorID,
							@new_amount =t.Amount,
							@new_4number = t.LastFourNumbersOfBankCard,
							@new_trackingNumber = t.TrackingNumber,
							@new_date = CAST(t.TransactionDate as date)
					from inserted t;
					--------------------------------------------------------------------------
					select @count = count(*) from Mehr.dbo.SponsorTransactions t
					where t.SponsorID = @new_sponsorid
						and t.Amount = @new_amount
						and t.LastFourNumbersOfBankCard = @new_4number
						and t.TrackingNumber = @new_trackingNumber
						and CAST(t.TransactionDate as date) = CAST(@new_date as date)
	
					if @count > 0
						begin
							select @check = -1
						end
					else
						begin
							select @count = count(*) from Mehr.dbo.BankDatas t
							where t.Amount = @new_amount
								and ( t.TrackingNumber like '%'+@new_trackingNumber+'%'
								or CAST(t.TransactionDate as date) = CAST(@new_date as date))

							if @count > 0
								begin
									select @check = 1 
								end
							else
								begin
									select @check = 0
								end
						end;
					--------------------------------------------------------------------------
					update Mehr.dbo.SponsorTransactions
					set isValid = @check
					where
						SponsorTransactionsID = @new_id
				END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                drop trigger validation;
            ");
        }
    }
}
