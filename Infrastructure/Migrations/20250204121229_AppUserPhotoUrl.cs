using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppUserPhotoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e572c94-6ffb-4c61-8174-a5a85860d750");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94cfedc3-b5ed-4e77-aaf8-77ae2f3f9ac5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6c66e4b4-4464-43aa-b423-9f13c3c0e35c", null, "User", "USER" },
                    { "bbc08a97-bc14-4a8b-b73e-493f6bd34ec8", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c66e4b4-4464-43aa-b423-9f13c3c0e35c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbc08a97-bc14-4a8b-b73e-493f6bd34ec8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e572c94-6ffb-4c61-8174-a5a85860d750", null, "User", "USER" },
                    { "94cfedc3-b5ed-4e77-aaf8-77ae2f3f9ac5", null, "Admin", "ADMIN" }
                });
        }
    }
}
