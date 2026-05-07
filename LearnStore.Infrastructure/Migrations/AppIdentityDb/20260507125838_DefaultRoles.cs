using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnStore.Infrastructure.Migrations.AppIdentityDb
{
    /// <inheritdoc />
    public partial class DefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "2752F10D-2938-4540-96F0-80F06F19F1D9", "Admin", "ADMIN" },
                    { "2", "DF2EBED8-6D69-4041-B4CE-63219305ECBC", "Customer", "CUSTOMER" },
                    { "3", "72AF9E5F-F92C-4BB1-949A-98302820ADB3", "Seller", "SELLER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");
        }
    }
}
