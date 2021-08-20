using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class modifyMainCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_MainCustomer_MainCustomerId1",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_MainCustomerId1",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MainCustomerId1",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "MainCustomerId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_MainCustomerId",
                table: "Employee",
                column: "MainCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_MainCustomer_MainCustomerId",
                table: "Employee",
                column: "MainCustomerId",
                principalTable: "MainCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_MainCustomer_MainCustomerId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_MainCustomerId",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "MainCustomerId",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MainCustomerId1",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_MainCustomerId1",
                table: "Employee",
                column: "MainCustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_MainCustomer_MainCustomerId1",
                table: "Employee",
                column: "MainCustomerId1",
                principalTable: "MainCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
