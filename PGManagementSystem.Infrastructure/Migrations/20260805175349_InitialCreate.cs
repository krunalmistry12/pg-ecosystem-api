using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ROLE_MST",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_MST", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_MST",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MST", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_USER_MST_ROLE_MST_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLE_MST",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FLAT_MST",
                columns: table => new
                {
                    FlatId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FlatNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApartmentName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FLAT_MST", x => x.FlatId);
                    table.ForeignKey(
                        name: "FK_FLAT_MST_USER_MST_UserId",
                        column: x => x.UserId,
                        principalTable: "USER_MST",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ZONE_MST",
                columns: table => new
                {
                    ZoneId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ZoneName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    RentPerBed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlatId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZONE_MST", x => x.ZoneId);
                    table.ForeignKey(
                        name: "FK_ZONE_MST_FLAT_MST_FlatId",
                        column: x => x.FlatId,
                        principalTable: "FLAT_MST",
                        principalColumn: "FlatId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BED_MST",
                columns: table => new
                {
                    BedId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BedNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZoneId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BED_MST", x => x.BedId);
                    table.ForeignKey(
                        name: "FK_BED_MST_ZONE_MST_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "ZONE_MST",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ROLE_MST",
                columns: new[] { "RoleId", "CreatedAt", "Description", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System Administrator with full access", true, "SuperAdmin" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PG Owner / Main Admin", true, "Admin" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PG Manager / Maintenance Staff", true, "Staff" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PG Tenant / Guest", true, "Tenant" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BED_MST_ZoneId",
                table: "BED_MST",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_FLAT_MST_FlatNumber_UserId",
                table: "FLAT_MST",
                columns: new[] { "FlatNumber", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FLAT_MST_UserId",
                table: "FLAT_MST",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_MST_Email",
                table: "USER_MST",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_MST_Phone",
                table: "USER_MST",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_MST_RoleId",
                table: "USER_MST",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ZONE_MST_FlatId",
                table: "ZONE_MST",
                column: "FlatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BED_MST");

            migrationBuilder.DropTable(
                name: "ZONE_MST");

            migrationBuilder.DropTable(
                name: "FLAT_MST");

            migrationBuilder.DropTable(
                name: "USER_MST");

            migrationBuilder.DropTable(
                name: "ROLE_MST");
        }
    }
}
