using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RoomRent",
                table: "ZONE_MST",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PricingType",
                table: "FLAT_MST",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "BedRent",
                table: "BED_MST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomRent",
                table: "ZONE_MST");

            migrationBuilder.DropColumn(
                name: "PricingType",
                table: "FLAT_MST");

            migrationBuilder.DropColumn(
                name: "BedRent",
                table: "BED_MST");
        }
    }
}
