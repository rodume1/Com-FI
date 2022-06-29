using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com_Fi.Data.Migrations
{
    public partial class changeDatabaseName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "d6103158-83e9-4ada-ac6a-82fb536e20cf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "00fbb071-b3ec-4493-951a-341bb14f5b17");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "2e7fe702-feeb-4dda-9c45-3dd5bb1a568d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "d7edff8f-f9ca-4883-b1a8-67c5c0ed9abd");
        }
    }
}
