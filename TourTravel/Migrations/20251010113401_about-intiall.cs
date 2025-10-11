using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourTravel.Migrations
{
    /// <inheritdoc />
    public partial class aboutintiall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "About",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "About",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SlugUrl",
                table: "About",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "About");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "About");

            migrationBuilder.DropColumn(
                name: "SlugUrl",
                table: "About");
        }
    }
}
