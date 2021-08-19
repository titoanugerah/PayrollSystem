using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Payroll.Migrations
{
    public partial class stringNIK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmploymentStatus_EmploymentStatusId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmploymentStatusId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BirthPlace",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DriverLicenseExpire",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmploymentStatusId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EndContract",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "HasIdCard",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "HasTraining",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "HasUniform",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdCardDeliveryDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JoinCompanyDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JoinCustomerDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "KK",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "StartContract",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TrainingDeliveryDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TrainingGrade",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TrainingName",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TrainingRemark",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UniformDeliveryDate",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Role",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Role",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Position",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Position",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "PayrollHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "PayrollHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "PayrollDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PayrollDetail",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "PayrollDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "MainCustomer",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "MainCustomer",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Location",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Location",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "FamilyStatus",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "FamilyStatus",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "EmploymentStatus",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "EmploymentStatus",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "Employee",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "District",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "District",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Customer",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Customer",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Bank",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Bank",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "Role",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Role",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "Position",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Position",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "PayrollHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "PayrollDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PayrollDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)");

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "PayrollDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "MainCustomer",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "MainCustomer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "Location",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "FamilyStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "FamilyStatus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "EmploymentStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "EmploymentStatus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NIK",
                table: "Employee",
                type: "int",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DriverLicenseExpire",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmploymentStatusId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndContract",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<ulong>(
                name: "HasIdCard",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "HasTraining",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "HasUniform",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<DateTime>(
                name: "IdCardDeliveryDate",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinCompanyDate",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinCustomerDate",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "KK",
                table: "Employee",
                type: "varchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Employee",
                type: "varchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartContract",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TrainingDeliveryDate",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TrainingGrade",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingName",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingRemark",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UniformDeliveryDate",
                table: "Employee",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "District",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "District",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "Customer",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifyBy",
                table: "Bank",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateBy",
                table: "Bank",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmploymentStatusId",
                table: "Employee",
                column: "EmploymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmploymentStatus_EmploymentStatusId",
                table: "Employee",
                column: "EmploymentStatusId",
                principalTable: "EmploymentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
