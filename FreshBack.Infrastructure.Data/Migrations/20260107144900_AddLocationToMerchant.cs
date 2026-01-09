using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FreshBack.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToMerchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Merchants");

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "Merchants",
                type: "geography",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.Sql(
                "CREATE SPATIAL INDEX IX_Merchants_Location ON Merchants(Location) USING GEOGRAPHY_AUTO_GRID"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Merchants",
                type: "decimal(18,18)",
                precision: 18,
                scale: 18,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Merchants",
                type: "decimal(18,18)",
                precision: 18,
                scale: 18,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
                "DROP INDEX IX_Merchants_Location ON Merchants"
            );
        }
    }
}
