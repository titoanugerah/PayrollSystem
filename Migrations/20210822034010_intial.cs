using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Payroll.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyStatus",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PTKP = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyStatus", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MainCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainCustomer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Keyword = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UMK = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    MainCustomerId = table.Column<int>(type: "int", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_MainCustomer_MainCustomerId",
                        column: x => x.MainCustomerId,
                        principalTable: "MainCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayrollHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MainCustomerId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    JamsostekPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BpjsPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PensionPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ManagementFeePercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PpnPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BpjsTk1Percentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BpjsKesehatanPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Pension1Percentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Pph21Percentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Pph23Percentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BpjsPayrollPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PensionPayrollPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollHistory_MainCustomer_MainCustomerId",
                        column: x => x.MainCustomerId,
                        principalTable: "MainCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PrimaryNIK = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    SecondaryNIK = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MainCustomerId = table.Column<int>(type: "int", nullable: false),
                    BpjsStatusId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    FamilyStatusCode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    BankCode = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    AccountName = table.Column<string>(type: "text", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Bank_BankCode",
                        column: x => x.BankCode,
                        principalTable: "Bank",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_FamilyStatus_FamilyStatusCode",
                        column: x => x.FamilyStatusCode,
                        principalTable: "FamilyStatus",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_MainCustomer_MainCustomerId",
                        column: x => x.MainCustomerId,
                        principalTable: "MainCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayrollDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PayrollHistoryId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AbsentDeduction = table.Column<int>(type: "int", nullable: false),
                    MainSalaryBilling = table.Column<int>(type: "int", nullable: false),
                    JamsostekBilling = table.Column<int>(type: "int", nullable: false),
                    BpjsBilling = table.Column<int>(type: "int", nullable: false),
                    PensionBilling = table.Column<int>(type: "int", nullable: false),
                    AtributeBilling = table.Column<int>(type: "int", nullable: false),
                    TrainingBilling = table.Column<int>(type: "int", nullable: false),
                    RouteBilling = table.Column<int>(type: "int", nullable: false),
                    Thr = table.Column<int>(type: "int", nullable: false),
                    Rapel = table.Column<int>(type: "int", nullable: false),
                    BpjsReturn = table.Column<int>(type: "int", nullable: false),
                    AppreciationBilling = table.Column<int>(type: "int", nullable: false),
                    TransferFee = table.Column<int>(type: "int", nullable: false),
                    IsLateTransfer = table.Column<ulong>(type: "bit", nullable: false),
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
                    PulseAllowance = table.Column<int>(type: "int", nullable: false),
                    AnotherDeductionRemark = table.Column<string>(type: "text", nullable: true),
                    TakeHomePay = table.Column<int>(type: "int", nullable: false),
                    PayrollDetailStatusId = table.Column<int>(type: "int", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollDetail_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollDetail_PayrollHistory_PayrollHistoryId",
                        column: x => x.PayrollHistoryId,
                        principalTable: "PayrollHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MainCustomerId",
                table: "Customer",
                column: "MainCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BankCode",
                table: "Employee",
                column: "BankCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CustomerId",
                table: "Employee",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_FamilyStatusCode",
                table: "Employee",
                column: "FamilyStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LocationId",
                table: "Employee",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_MainCustomerId",
                table: "Employee",
                column: "MainCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PositionId",
                table: "Employee",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_RoleId",
                table: "Employee",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_DistrictId",
                table: "Location",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDetail_EmployeeId",
                table: "PayrollDetail",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDetail_PayrollHistoryId",
                table: "PayrollDetail",
                column: "PayrollHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollHistory_MainCustomerId",
                table: "PayrollHistory",
                column: "MainCustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollDetail");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "PayrollHistory");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "FamilyStatus");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "MainCustomer");

            migrationBuilder.DropTable(
                name: "District");
        }
    }
}
