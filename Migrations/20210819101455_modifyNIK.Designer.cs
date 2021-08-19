﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Payroll.DataAccess;

namespace Payroll.Migrations
{
    [DbContext(typeof(PayrollDB))]
    [Migration("20210819101455_modifyNIK")]
    partial class modifyNIK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("Payroll.Models.Bank", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("Payroll.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int>("MainCustomerId")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MainCustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Payroll.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("District");
                });

            modelBuilder.Entity("Payroll.Models.Employee", b =>
                {
                    b.Property<string>("NIK")
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.Property<string>("AccountName")
                        .HasColumnType("text");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("text");

                    b.Property<string>("BankCode")
                        .HasColumnType("varchar(12)");

                    b.Property<string>("BpjsNumber")
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)");

                    b.Property<string>("BpjsRemark")
                        .HasColumnType("text");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("DriverLicense")
                        .HasColumnType("text");

                    b.Property<string>("DriverLicenseType")
                        .HasColumnType("text");

                    b.Property<string>("FamilyStatusCode")
                        .HasColumnType("varchar(2)");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("JamsostekNumber")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("JamsostekRemark")
                        .HasColumnType("text");

                    b.Property<string>("KTP")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("NPWP")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("NIK");

                    b.HasIndex("BankCode");

                    b.HasIndex("CustomerId");

                    b.HasIndex("FamilyStatusCode");

                    b.HasIndex("LocationId");

                    b.HasIndex("PositionId");

                    b.HasIndex("RoleId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Payroll.Models.EmploymentStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EmploymentStatus");
                });

            modelBuilder.Entity("Payroll.Models.FamilyStatus", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PTKP")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("FamilyStatus");
                });

            modelBuilder.Entity("Payroll.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UMK")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Payroll.Models.MainCustomer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MainCustomer");
                });

            modelBuilder.Entity("Payroll.Models.PayrollDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AbsentDeduction")
                        .HasColumnType("int");

                    b.Property<int>("AnotherDeduction")
                        .HasColumnType("int");

                    b.Property<string>("AnotherDeductionRemark")
                        .HasColumnType("text");

                    b.Property<int>("AppreciationBilling")
                        .HasColumnType("int");

                    b.Property<int>("AtributeBilling")
                        .HasColumnType("int");

                    b.Property<int>("AttendanceBilling")
                        .HasColumnType("int");

                    b.Property<int>("AttributePayroll")
                        .HasColumnType("int");

                    b.Property<int>("BpjsBilling")
                        .HasColumnType("int");

                    b.Property<int>("BpjsKesehatanDeduction")
                        .HasColumnType("int");

                    b.Property<int>("BpjsReturn")
                        .HasColumnType("int");

                    b.Property<int>("BpjsTkDeduction")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<int>("FeePayroll")
                        .HasColumnType("int");

                    b.Property<int>("GrandTotalBilling")
                        .HasColumnType("int");

                    b.Property<int>("GrossPayroll")
                        .HasColumnType("int");

                    b.Property<int>("InsentiveBilling")
                        .HasColumnType("int");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<ulong>("IsLateTransfer")
                        .HasColumnType("bit");

                    b.Property<int>("JamsostekBilling")
                        .HasColumnType("int");

                    b.Property<int>("MainPrice")
                        .HasColumnType("int");

                    b.Property<int>("MainSalaryBilling")
                        .HasColumnType("int");

                    b.Property<int>("ManagementFeeBilling")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<int>("Netto")
                        .HasColumnType("int");

                    b.Property<int>("OvertimeBilling")
                        .HasColumnType("int");

                    b.Property<int>("PKP1")
                        .HasColumnType("int");

                    b.Property<int>("PKP2")
                        .HasColumnType("int");

                    b.Property<int>("PPH21")
                        .HasColumnType("int");

                    b.Property<int>("PPH23")
                        .HasColumnType("int");

                    b.Property<int>("PTKP")
                        .HasColumnType("int");

                    b.Property<int>("PayrollDetailStatusId")
                        .HasColumnType("int");

                    b.Property<int>("PayrollHistoryId")
                        .HasColumnType("int");

                    b.Property<int>("PensionBilling")
                        .HasColumnType("int");

                    b.Property<int>("PensionDeduction")
                        .HasColumnType("int");

                    b.Property<int>("PulseAllowance")
                        .HasColumnType("int");

                    b.Property<int>("Rapel")
                        .HasColumnType("int");

                    b.Property<int>("ResultPayroll")
                        .HasColumnType("int");

                    b.Property<int>("SubtotalBilling")
                        .HasColumnType("int");

                    b.Property<int>("TakeHomePay")
                        .HasColumnType("int");

                    b.Property<int>("TaxBilling")
                        .HasColumnType("int");

                    b.Property<int>("TaxPayroll")
                        .HasColumnType("int");

                    b.Property<int>("TotalPayroll")
                        .HasColumnType("int");

                    b.Property<int>("TransferFee")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PayrollHistoryId");

                    b.ToTable("PayrollDetail");
                });

            modelBuilder.Entity("Payroll.Models.PayrollHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("BpjsKesehatanPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("BpjsPayrollPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("BpjsPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("BpjsTk1Percentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<decimal>("JamsostekPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("ManagementFeePercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Pension1Percentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("PensionPayrollPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("PensionPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Pph21Percentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Pph23Percentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("PpnPercentage")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PayrollHistory");
                });

            modelBuilder.Entity("Payroll.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("Payroll.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Payroll.Models.Customer", b =>
                {
                    b.HasOne("Payroll.Models.MainCustomer", "MainCustomer")
                        .WithMany()
                        .HasForeignKey("MainCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainCustomer");
                });

            modelBuilder.Entity("Payroll.Models.Employee", b =>
                {
                    b.HasOne("Payroll.Models.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankCode");

                    b.HasOne("Payroll.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.FamilyStatus", "FamilyStatus")
                        .WithMany()
                        .HasForeignKey("FamilyStatusCode");

                    b.HasOne("Payroll.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("Customer");

                    b.Navigation("FamilyStatus");

                    b.Navigation("Location");

                    b.Navigation("Position");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Payroll.Models.Location", b =>
                {
                    b.HasOne("Payroll.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Payroll.Models.PayrollDetail", b =>
                {
                    b.HasOne("Payroll.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.PayrollHistory", "PayrollHistory")
                        .WithMany()
                        .HasForeignKey("PayrollHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("PayrollHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
