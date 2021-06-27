using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class removeRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KK",
                table: "Employee",
                type: "varchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "JamsostekNumber",
                table: "Employee",
                type: "varchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "BpjsNumber",
                table: "Employee",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KK",
                table: "Employee",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JamsostekNumber",
                table: "Employee",
                type: "varchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BpjsNumber",
                table: "Employee",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14,
                oldNullable: true);
        }
    }
}
