using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class ttnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteBilling",
                table: "PayrollDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainingBilling",
                table: "PayrollDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteBilling",
                table: "PayrollDetail");

            migrationBuilder.DropColumn(
                name: "TrainingBilling",
                table: "PayrollDetail");
        }
    }
}
