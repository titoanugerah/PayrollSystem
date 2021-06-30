using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class resolvename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollStation",
                table: "PayrollStation");

            migrationBuilder.RenameTable(
                name: "PayrollStation",
                newName: "PayrollStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollStatus",
                table: "PayrollStatus",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollStatus",
                table: "PayrollStatus");

            migrationBuilder.RenameTable(
                name: "PayrollStatus",
                newName: "PayrollStation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollStation",
                table: "PayrollStation",
                column: "Id");
        }
    }
}
