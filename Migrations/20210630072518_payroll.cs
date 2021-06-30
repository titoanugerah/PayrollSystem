using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Payroll.Migrations
{
    public partial class payroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayrollHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Month = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    JamsostekPercentage = table.Column<int>(type: "int", nullable: false),
                    BpjsPercentage = table.Column<int>(type: "int", nullable: false),
                    PensionPercentage = table.Column<int>(type: "int", nullable: false),
                    ManagementFeePercentage = table.Column<int>(type: "int", nullable: false),
                    PpnPercentage = table.Column<int>(type: "int", nullable: false),
                    BpjsTk1Percentage = table.Column<int>(type: "int", nullable: false),
                    BpjsKesehatanPercentage = table.Column<int>(type: "int", nullable: false),
                    Pension1Percentage = table.Column<int>(type: "int", nullable: false),
                    Pph21Percentage = table.Column<int>(type: "int", nullable: false),
                    Pph23Percentage = table.Column<int>(type: "int", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollStation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PayrollHistoryId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    MainSalaryBilling = table.Column<int>(type: "int", nullable: false),
                    JamsostekBilling = table.Column<int>(type: "int", nullable: false),
                    BpjsBilling = table.Column<int>(type: "int", nullable: false),
                    PensionBilling = table.Column<int>(type: "int", nullable: false),
                    AtributeBilling = table.Column<int>(type: "int", nullable: false),
                    MainPrice = table.Column<int>(type: "int", nullable: false),
                    ManagementFeeBilling = table.Column<int>(type: "int", nullable: false),
                    InsentiveBilling = table.Column<int>(type: "int", nullable: false),
                    AttendanceBilling = table.Column<int>(type: "int", nullable: false),
                    OvertimeBilling = table.Column<int>(type: "int", nullable: false),
                    SubtotalBilling = table.Column<int>(type: "int", nullable: false),
                    TaxBilling = table.Column<int>(type: "int", nullable: false),
                    GrandTotalBilling = table.Column<int>(type: "int", nullable: false),
                    ResultPayroll = table.Column<int>(type: "int", nullable: false),
                    FeePayroll = table.Column<int>(type: "int", nullable: false),
                    TotalPayroll = table.Column<int>(type: "int", nullable: false),
                    TaxPayroll = table.Column<int>(type: "int", nullable: false),
                    GrossPayroll = table.Column<int>(type: "int", nullable: false),
                    AttributePayroll = table.Column<int>(type: "int", nullable: false),
                    BpjsTkDeduction = table.Column<int>(type: "int", nullable: false),
                    BpjsKesehatanDeduction = table.Column<int>(type: "int", nullable: false),
                    PensionDeduction = table.Column<int>(type: "int", nullable: false),
                    PKP1 = table.Column<int>(type: "int", nullable: false),
                    PTKP = table.Column<int>(type: "int", nullable: false),
                    PKP2 = table.Column<int>(type: "int", nullable: false),
                    PPH21 = table.Column<int>(type: "int", nullable: false),
                    PPH23 = table.Column<int>(type: "int", nullable: false),
                    Netto = table.Column<int>(type: "int", nullable: false),
                    AnotherDeduction = table.Column<int>(type: "int", nullable: false),
                    AnotherDeductionRemark = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollDetail_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollDetail_PayrollHistory_PayrollHistoryId",
                        column: x => x.PayrollHistoryId,
                        principalTable: "PayrollHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDetail_EmployeeId",
                table: "PayrollDetail",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDetail_PayrollHistoryId",
                table: "PayrollDetail",
                column: "PayrollHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollDetail");

            migrationBuilder.DropTable(
                name: "PayrollStation");

            migrationBuilder.DropTable(
                name: "PayrollHistory");
        }
    }
}
