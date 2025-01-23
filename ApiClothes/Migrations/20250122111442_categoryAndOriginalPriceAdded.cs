using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiClothes.Migrations
{
    /// <inheritdoc />
    public partial class categoryAndOriginalPriceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Announcements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Announcements");
        }
    }
}
