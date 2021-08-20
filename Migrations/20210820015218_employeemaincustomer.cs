using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class employeemaincustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainCustomerId",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainCustomerId1",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollHistory_MainCustomerId",
                table: "PayrollHistory",
                column: "MainCustomerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollHistory_MainCustomer_MainCustomerId",
                table: "PayrollHistory",
                column: "MainCustomerId",
                principalTable: "MainCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_MainCustomer_MainCustomerId1",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollHistory_MainCustomer_MainCustomerId",
                table: "PayrollHistory");

            migrationBuilder.DropIndex(
                name: "IX_PayrollHistory_MainCustomerId",
                table: "PayrollHistory");

            migrationBuilder.DropIndex(
                name: "IX_Employee_MainCustomerId1",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MainCustomerId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MainCustomerId1",
                table: "Employee");
        }
    }
}
