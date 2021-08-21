using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PayrollDetail",
                type: "varchar(8)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(6)");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "Employee",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(6)",
                oldMaxLength: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PayrollDetail",
                type: "varchar(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "Employee",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldMaxLength: 8);
        }
    }
}
