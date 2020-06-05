﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataLayer.Bank", b =>
                {
                    b.Property<int>("BankID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccountNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<long>("CardNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ShebaNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(26)")
                        .HasMaxLength(26);

                    b.Property<string>("pic")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("BankID");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("DataLayer.BankData", b =>
                {
                    b.Property<int>("BankDataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("LastFourNumbersOfBankCard")
                        .HasColumnType("int");

                    b.Property<string>("TrackingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BankDataID");

                    b.ToTable("BankData");
                });

            modelBuilder.Entity("DataLayer.BankTransaction", b =>
                {
                    b.Property<int>("BankTransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BankID")
                        .HasColumnType("int");

                    b.Property<int?>("TransactionBankDataID")
                        .HasColumnType("int");

                    b.HasKey("BankTransactionID");

                    b.HasIndex("BankID");

                    b.HasIndex("TransactionBankDataID");

                    b.ToTable("BankTransactions");
                });

            modelBuilder.Entity("DataLayer.Colleague", b =>
                {
                    b.Property<int>("ColleagueID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartActivity")
                        .HasColumnType("datetime2");

                    b.Property<int>("code")
                        .HasColumnType("int");

                    b.Property<string>("color")
                        .HasColumnType("nvarchar(7)")
                        .HasMaxLength(7);

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.Property<bool>("isMale")
                        .HasColumnType("bit");

                    b.Property<string>("picName")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("ColleagueID");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Colleagues");
                });

            modelBuilder.Entity("DataLayer.Models.ReceiptData", b =>
                {
                    b.Property<int>("ReceiptDataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<long>("ReceiptNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ReceiptDataID");

                    b.ToTable("ReceiptData");
                });

            modelBuilder.Entity("DataLayer.Sponsor", b =>
                {
                    b.Property<int>("SponsorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CauseOfSupport")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("ColleagueID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OtherSupport")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.Property<bool>("isMale")
                        .HasColumnType("bit");

                    b.Property<string>("picName")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("SponsorID");

                    b.HasIndex("ColleagueID");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Sponsors");
                });

            modelBuilder.Entity("DataLayer.SponsorTransaction", b =>
                {
                    b.Property<int>("SponsorTransactionsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CauseOfSupport")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("ColleagueID")
                        .HasColumnType("int");

                    b.Property<int?>("MyReceiptReceiptDataID")
                        .HasColumnType("int");

                    b.Property<int?>("MyTransactionBankDataID")
                        .HasColumnType("int");

                    b.Property<string>("OtherSupport")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("SponsorID")
                        .HasColumnType("int");

                    b.Property<bool>("isValid")
                        .HasColumnType("bit");

                    b.HasKey("SponsorTransactionsID");

                    b.HasIndex("MyReceiptReceiptDataID");

                    b.HasIndex("MyTransactionBankDataID");

                    b.HasIndex("SponsorID");

                    b.ToTable("SponsorTransactions");
                });

            modelBuilder.Entity("DataLayer.SponsorTransactionError", b =>
                {
                    b.Property<int>("ErrorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ColleagueID")
                        .HasColumnType("int");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiptNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SponsorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrackingNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ErrorID");

                    b.HasIndex("ColleagueID");

                    b.ToTable("Errors");
                });

            modelBuilder.Entity("DataLayer.BankTransaction", b =>
                {
                    b.HasOne("DataLayer.Bank", "MyBank")
                        .WithMany("Transactions")
                        .HasForeignKey("BankID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.BankData", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionBankDataID");
                });

            modelBuilder.Entity("DataLayer.Sponsor", b =>
                {
                    b.HasOne("DataLayer.Colleague", "MyColleague")
                        .WithMany("Sponsors")
                        .HasForeignKey("ColleagueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.SponsorTransaction", b =>
                {
                    b.HasOne("DataLayer.Models.ReceiptData", "MyReceipt")
                        .WithMany()
                        .HasForeignKey("MyReceiptReceiptDataID");

                    b.HasOne("DataLayer.BankData", "MyTransaction")
                        .WithMany()
                        .HasForeignKey("MyTransactionBankDataID");

                    b.HasOne("DataLayer.Sponsor", "MySponsor")
                        .WithMany("MyTransactions")
                        .HasForeignKey("SponsorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.SponsorTransactionError", b =>
                {
                    b.HasOne("DataLayer.Colleague", null)
                        .WithMany("Errors")
                        .HasForeignKey("ColleagueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
