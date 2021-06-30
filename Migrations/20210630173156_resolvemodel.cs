using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class resolvemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PpnPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Pph23Percentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Pph21Percentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PensionPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Pension1Percentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ManagementFeePercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "JamsostekPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "BpjsTk1Percentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "BpjsPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "BpjsKesehatanPercentage",
                table: "PayrollHistory",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollHistory_StatusId",
                table: "PayrollHistory",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollHistory_PayrollStatus_StatusId",
                table: "PayrollHistory",
                column: "StatusId",
                principalTable: "PayrollStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollHistory_PayrollStatus_StatusId",
                table: "PayrollHistory");

            migrationBuilder.DropIndex(
                name: "IX_PayrollHistory_StatusId",
                table: "PayrollHistory");

            migrationBuilder.AlterColumn<int>(
                name: "PpnPercentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "Pph23Percentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "Pph21Percentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "PensionPercentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "Pension1Percentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "ManagementFeePercentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "JamsostekPercentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "BpjsTk1Percentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "BpjsPercentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "BpjsKesehatanPercentage",
                table: "PayrollHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }
    }
}
