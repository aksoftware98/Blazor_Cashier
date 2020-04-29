using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCashier.Server.Migrations
{
    public partial class EditInvoiceItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: "69c4dd5e-c804-49ad-9216-c7f8b9a20d33");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: "e5909d88-be72-462c-ad68-47ac448d20a6");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceItems");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "1f9e24e2-6cd1-4a47-9625-8f7cb8f57a80", "LB", new DateTimeOffset(new DateTime(2020, 4, 24, 7, 35, 54, 163, DateTimeKind.Unspecified).AddTicks(8362), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 24, 7, 35, 54, 163, DateTimeKind.Unspecified).AddTicks(8387), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "34c16105-1b24-4fb3-8cab-96543943fe92", "USD", new DateTimeOffset(new DateTime(2020, 4, 24, 7, 35, 54, 165, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 24, 7, 35, 54, 165, DateTimeKind.Unspecified).AddTicks(3645), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }
    }
}
