using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70e88a65-300f-4ac7-a07f-63dbebca25e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c649c0fd-5110-41ae-8e35-267d8e3f0b01");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "Transactions",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45de9af7-4e30-4d1f-ad41-a04832dd923c", null, "Admin", "ADMIN" },
                    { "d1b6fa0f-a59d-4c96-9e27-7fe4f93dc605", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45de9af7-4e30-4d1f-ad41-a04832dd923c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1b6fa0f-a59d-4c96-9e27-7fe4f93dc605");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "Transactions",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "70e88a65-300f-4ac7-a07f-63dbebca25e4", null, "Admin", "ADMIN" },
                    { "c649c0fd-5110-41ae-8e35-267d8e3f0b01", null, "User", "USER" }
                });
        }
    }
}
