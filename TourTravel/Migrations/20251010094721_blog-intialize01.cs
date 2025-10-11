using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourTravel.Migrations
{
    /// <inheritdoc />
    public partial class blogintialize01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Blog",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Blog",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SlugUrl",
                table: "Blog",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "SlugUrl",
                table: "Blog");
        }
    }
}
