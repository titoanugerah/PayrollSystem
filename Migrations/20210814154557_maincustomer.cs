using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Payroll.Migrations
{
    public partial class maincustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainCustomerId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 1);

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

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MainCustomerId",
                table: "Customer",
                column: "MainCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_MainCustomer_MainCustomerId",
                table: "Customer",
                column: "MainCustomerId",
                principalTable: "MainCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_MainCustomer_MainCustomerId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "MainCustomer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_MainCustomerId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "MainCustomerId",
                table: "Customer");
        }
    }
}
