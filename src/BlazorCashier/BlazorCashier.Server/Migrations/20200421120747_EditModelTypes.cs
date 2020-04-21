using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCashier.Server.Migrations
{
    public partial class EditModelTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Countries",
            //    keyColumn: "Id",
            //    keyValue: "7cd2931e-1927-4a90-b8d7-c143ec9f47af");

            //migrationBuilder.DeleteData(
            //    table: "Currencies",
            //    keyColumn: "Id",
            //    keyValue: "f9703357-0fe2-4b83-9aac-3f98b6e4e952");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "Stocks",
                type: "decimal(8, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Stocks",
                type: "decimal(8, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<float>(
                name: "Discount",
                table: "InvoiceItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Discounts",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Discounts",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Bills",
                type: "decimal(8, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BillItems",
                type: "decimal(8, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "d257efc0-76fb-47f0-9ee1-fea8767cb04e", "LB", new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 290, DateTimeKind.Unspecified).AddTicks(3257), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 290, DateTimeKind.Unspecified).AddTicks(3286), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "20023616-9487-4c1a-aae9-c95f6cc918b2", "USD", new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 292, DateTimeKind.Unspecified).AddTicks(1605), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 21, 12, 7, 46, 292, DateTimeKind.Unspecified).AddTicks(1624), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: "d257efc0-76fb-47f0-9ee1-fea8767cb04e");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: "20023616-9487-4c1a-aae9-c95f6cc918b2");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "Points",
                table: "Stocks",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RegistrationDate",
                table: "Organizations",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "StartDate",
                table: "Discounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EndDate",
                table: "Discounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BillItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 2)");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "7cd2931e-1927-4a90-b8d7-c143ec9f47af", "LB", new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 533, DateTimeKind.Unspecified).AddTicks(4528), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 533, DateTimeKind.Unspecified).AddTicks(4552), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "f9703357-0fe2-4b83-9aac-3f98b6e4e952", "USD", new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 534, DateTimeKind.Unspecified).AddTicks(9143), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 534, DateTimeKind.Unspecified).AddTicks(9154), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }
    }
}
