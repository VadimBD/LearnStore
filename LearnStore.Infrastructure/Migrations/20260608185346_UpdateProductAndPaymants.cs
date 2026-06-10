using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductAndPaymants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileStorageName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileStorageName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payment");
        }
    }
}
