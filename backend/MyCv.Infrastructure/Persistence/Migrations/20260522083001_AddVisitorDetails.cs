using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCv.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "PageVisits",
                type: "text",
                nullable: true);

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
                name: "Browser",
                table: "CvDownloads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "CvDownloads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CvDownloads",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "UtmMedium",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "UtmSource",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "Browser",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "UtmMedium",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "UtmSource",
                table: "CvDownloads");
        }
    }
}
