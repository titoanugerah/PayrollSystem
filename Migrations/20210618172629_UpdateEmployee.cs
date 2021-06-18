using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class UpdateEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Employee",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Employee");
        }
    }
}
