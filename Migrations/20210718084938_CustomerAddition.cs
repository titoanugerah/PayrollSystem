using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class CustomerAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseType",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IdCardDeliveryDate",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverLicenseType",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdCardDeliveryDate",
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
        }
    }
}
