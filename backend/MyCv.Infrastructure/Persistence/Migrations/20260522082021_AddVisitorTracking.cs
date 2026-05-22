using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCv.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FingerprintId",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "PageVisits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CvDownloads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FingerprintId",
                table: "CvDownloads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "CvDownloads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "CvDownloads",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "FingerprintId",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "PageVisits");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "FingerprintId",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "CvDownloads");

            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "CvDownloads");
        }
    }
}
