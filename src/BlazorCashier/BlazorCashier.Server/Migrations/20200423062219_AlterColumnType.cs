using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCashier.Server.Migrations
{
    public partial class AlterColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "Discounts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: "8b653f55-60d4-4b2d-a8dc-2e9c2b8dea57");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: "936b2aee-7ac7-4c71-8eb6-21fc33f95e69");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Discounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "42d0f5f0-f86f-48c2-9c15-b1a9f0b2ded3", "LB", new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 801, DateTimeKind.Unspecified).AddTicks(6938), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 801, DateTimeKind.Unspecified).AddTicks(6967), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "dcedbfad-f2ef-43a0-9829-af4462108bf3", "USD", new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 803, DateTimeKind.Unspecified).AddTicks(2836), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 803, DateTimeKind.Unspecified).AddTicks(2850), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }
    }
}
