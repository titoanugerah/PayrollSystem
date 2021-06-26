using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Payroll.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
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
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UMK = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentStatus",
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
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentStatus", x => x.Id);
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
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyStatus", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Position",
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
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
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
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
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
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
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
                name: "Employee",
                columns: table => new
                {
                    NIK = table.Column<int>(type: "int", maxLength: 5, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sex = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true),
                    BirthPlace = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Religion = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    KTP = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    KK = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    DriverLicense = table.Column<string>(type: "text", nullable: true),
                    DriverLicenseExpire = table.Column<DateTime>(type: "datetime", nullable: false),
                    FamilyStatusCode = table.Column<string>(type: "varchar(2)", nullable: false),
                    BpjsNumber = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    BpjsRemark = table.Column<string>(type: "text", nullable: true),
                    JamsostekNumber = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    JamsostekRemark = table.Column<string>(type: "text", nullable: true),
                    NPWP = table.Column<string>(type: "text", nullable: true),
                    JoinCompanyDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartContract = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndContract = table.Column<DateTime>(type: "datetime", nullable: false),
                    BankCode = table.Column<string>(type: "varchar(4)", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    AccountName = table.Column<string>(type: "text", nullable: false),
                    EmploymentStatusId = table.Column<int>(type: "int", nullable: false),
                    HasUniform = table.Column<ulong>(type: "bit", nullable: false),
                    HasIdCard = table.Column<ulong>(type: "bit", nullable: false),
                    HasTraining = table.Column<ulong>(type: "bit", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    JoinCustomerDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsExist = table.Column<ulong>(type: "bit", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreateDateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyDateUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_Employee_Bank_BankCode",
                        column: x => x.BankCode,
                        principalTable: "Bank",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_EmploymentStatus_EmploymentStatusId",
                        column: x => x.EmploymentStatusId,
                        principalTable: "EmploymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_FamilyStatus_FamilyStatusCode",
                        column: x => x.FamilyStatusCode,
                        principalTable: "FamilyStatus",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
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

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BankCode",
                table: "Employee",
                column: "BankCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CustomerId",
                table: "Employee",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmploymentStatusId",
                table: "Employee",
                column: "EmploymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_FamilyStatusCode",
                table: "Employee",
                column: "FamilyStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LocationId",
                table: "Employee",
                column: "LocationId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "EmploymentStatus");

            migrationBuilder.DropTable(
                name: "FamilyStatus");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "District");
        }
    }
}
