using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCv.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUtmColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtmMedium",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "UtmSource",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "UtmMedium",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "UtmSource",
                table: "CvDownloads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UtmMedium",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtmSource",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtmMedium",
                table: "CvDownloads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtmSource",
                table: "CvDownloads",
                type: "text",
                nullable: true);
        }
    }
}
