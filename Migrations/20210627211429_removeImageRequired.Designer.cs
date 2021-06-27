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
    [Migration("20210627211429_removeImageRequired")]
    partial class removeImageRequired
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
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Payroll.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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
                    b.Property<int>("NIK")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BankCode")
                        .IsRequired()
                        .HasColumnType("varchar(4)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime");

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BpjsNumber")
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)");

                    b.Property<string>("BpjsRemark")
                        .HasColumnType("text");

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("DriverLicense")
                        .HasColumnType("text");

                    b.Property<DateTime>("DriverLicenseExpire")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EmploymentStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndContract")
                        .HasColumnType("datetime");

                    b.Property<string>("FamilyStatusCode")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<ulong>("HasIdCard")
                        .HasColumnType("bit");

                    b.Property<ulong>("HasTraining")
                        .HasColumnType("bit");

                    b.Property<ulong>("HasUniform")
                        .HasColumnType("bit");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<string>("JamsostekNumber")
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<string>("JamsostekRemark")
                        .HasColumnType("text");

                    b.Property<DateTime>("JoinCompanyDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("JoinCustomerDate")
                        .HasColumnType("datetime");

                    b.Property<string>("KK")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("KTP")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifyDateUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("NPWP")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.Property<string>("Religion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Sex")
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.Property<DateTime>("StartContract")
                        .HasColumnType("datetime");

                    b.HasKey("NIK");

                    b.HasIndex("BankCode");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmploymentStatusId");

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

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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

            modelBuilder.Entity("Payroll.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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

                    b.Property<int>("CreateBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("datetime");

                    b.Property<ulong>("IsExist")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifyBy")
                        .HasColumnType("int");

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

            modelBuilder.Entity("Payroll.Models.Employee", b =>
                {
                    b.HasOne("Payroll.Models.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.EmploymentStatus", "EmploymentStatus")
                        .WithMany()
                        .HasForeignKey("EmploymentStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payroll.Models.FamilyStatus", "FamilyStatus")
                        .WithMany()
                        .HasForeignKey("FamilyStatusCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

                    b.Navigation("EmploymentStatus");

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
#pragma warning restore 612, 618
        }
    }
}
