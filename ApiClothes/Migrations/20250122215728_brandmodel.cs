using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiClothes.Migrations
{
    /// <inheritdoc />
    public partial class brandmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Announcements");
        }
    }
}
