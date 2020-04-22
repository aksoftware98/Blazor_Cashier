using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCashier.Server.Migrations
{
    public partial class UpdateOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastBillNumber",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastInvoiceNumber",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "42d0f5f0-f86f-48c2-9c15-b1a9f0b2ded3", "LB", new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 801, DateTimeKind.Unspecified).AddTicks(6938), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 801, DateTimeKind.Unspecified).AddTicks(6967), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "dcedbfad-f2ef-43a0-9829-af4462108bf3", "USD", new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 803, DateTimeKind.Unspecified).AddTicks(2836), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 21, 14, 49, 50, 803, DateTimeKind.Unspecified).AddTicks(2850), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: "42d0f5f0-f86f-48c2-9c15-b1a9f0b2ded3");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: "dcedbfad-f2ef-43a0-9829-af4462108bf3");

            migrationBuilder.DropColumn(
                name: "LastBillNumber",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LastInvoiceNumber",
                table: "Organizations");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "d257efc0-76fb-47f0-9ee1-fea8767cb04e", "LB", new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 290, DateTimeKind.Unspecified).AddTicks(3257), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 290, DateTimeKind.Unspecified).AddTicks(3286), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "20023616-9487-4c1a-aae9-c95f6cc918b2", "USD", new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 292, DateTimeKind.Unspecified).AddTicks(1605), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 292, DateTimeKind.Unspecified).AddTicks(1624), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }
    }
}
