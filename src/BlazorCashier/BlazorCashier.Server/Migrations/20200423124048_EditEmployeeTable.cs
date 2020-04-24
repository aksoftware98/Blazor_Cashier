using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCashier.Server.Migrations
{
    public partial class EditEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                 name: "FirstName",
                 table: "Employees",
                 type: "nvarchar(max)",
                 nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
