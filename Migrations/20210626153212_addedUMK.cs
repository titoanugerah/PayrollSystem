using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class addedUMK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UMK",
                table: "District");

            migrationBuilder.AddColumn<int>(
                name: "UMK",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UMK",
                table: "Location");

            migrationBuilder.AddColumn<int>(
                name: "UMK",
                table: "District",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
