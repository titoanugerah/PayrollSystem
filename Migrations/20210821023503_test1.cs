using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PayrollDetail",
                type: "varchar(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "Employee",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldMaxLength: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PayrollDetail",
                type: "varchar(8)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "Employee",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16);
        }
    }
}
