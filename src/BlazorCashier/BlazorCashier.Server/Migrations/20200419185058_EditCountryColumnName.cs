using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCashier.Server.Migrations
{
    public partial class EditCountryColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: "93e48ee8-fb66-40fb-be8c-9d27e4d1948d");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: "63ca1492-e03a-4aab-8fbd-e08e7d8a0fd6");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "7cd2931e-1927-4a90-b8d7-c143ec9f47af", "LB", new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 533, DateTimeKind.Unspecified).AddTicks(4528), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 533, DateTimeKind.Unspecified).AddTicks(4552), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "f9703357-0fe2-4b83-9aac-3f98b6e4e952", "USD", new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 534, DateTimeKind.Unspecified).AddTicks(9143), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 19, 18, 50, 57, 534, DateTimeKind.Unspecified).AddTicks(9154), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CountryId",
                table: "Organizations",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Countries_CountryId",
                table: "Organizations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Countries_CountryId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CountryId",
                table: "Organizations");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: "7cd2931e-1927-4a90-b8d7-c143ec9f47af");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: "f9703357-0fe2-4b83-9aac-3f98b6e4e952");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedDate", "CultureCode", "Flag", "LastModifiedDate", "Name" },
                values: new object[] { "93e48ee8-fb66-40fb-be8c-9d27e4d1948d", "LB", new DateTimeOffset(new DateTime(2020, 4, 18, 9, 27, 37, 821, DateTimeKind.Unspecified).AddTicks(1644), new TimeSpan(0, 0, 0, 0, 0)), "ar-LB", null, new DateTimeOffset(new DateTime(2020, 4, 18, 9, 27, 37, 821, DateTimeKind.Unspecified).AddTicks(1669), new TimeSpan(0, 0, 0, 0, 0)), "Lebanon" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedDate", "LastModifiedDate", "Name", "Symbol" },
                values: new object[] { "63ca1492-e03a-4aab-8fbd-e08e7d8a0fd6", "USD", new DateTimeOffset(new DateTime(2020, 4, 18, 9, 27, 37, 823, DateTimeKind.Unspecified).AddTicks(6275), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 4, 18, 9, 27, 37, 823, DateTimeKind.Unspecified).AddTicks(6286), new TimeSpan(0, 0, 0, 0, 0)), "Dollar", "$" });
        }
    }
}
