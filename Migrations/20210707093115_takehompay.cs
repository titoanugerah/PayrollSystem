using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class takehompay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BpjsPayrollPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PensionPayrollPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TakeHomePay",
                table: "PayrollDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BpjsPayrollPercentage",
                table: "PayrollHistory");

            migrationBuilder.DropColumn(
                name: "PensionPayrollPercentage",
                table: "PayrollHistory");

            migrationBuilder.DropColumn(
                name: "TakeHomePay",
                table: "PayrollDetail");
        }
    }
}
