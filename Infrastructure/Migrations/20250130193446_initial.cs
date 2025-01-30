using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45de9af7-4e30-4d1f-ad41-a04832dd923c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1b6fa0f-a59d-4c96-9e27-7fe4f93dc605");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4f3643c2-ca72-4b7f-8916-c0be220f9652", null, "User", "USER" },
                    { "7a9de53f-7d32-4a61-87cc-5a2bbf9f56f3", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f3643c2-ca72-4b7f-8916-c0be220f9652");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a9de53f-7d32-4a61-87cc-5a2bbf9f56f3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45de9af7-4e30-4d1f-ad41-a04832dd923c", null, "Admin", "ADMIN" },
                    { "d1b6fa0f-a59d-4c96-9e27-7fe4f93dc605", null, "User", "USER" }
                });
        }
    }
}
