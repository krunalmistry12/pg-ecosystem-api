using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreshTenantMasterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TENANTS_MST",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropertyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoomId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AllocationType = table.Column<int>(type: "int", nullable: false),
                    Rent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmergencyPhone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdvancePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DueDate = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartingMeterReading = table.Column<double>(type: "double", nullable: true),
                    LockInPeriodMonths = table.Column<int>(type: "int", nullable: false),
                    AgreementEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IdProofType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdProofNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdProofUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantPhotoUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PoliceVerificationStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VacatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TENANTS_MST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TENANTS_MST_BED_MST_BedId",
                        column: x => x.BedId,
                        principalTable: "BED_MST",
                        principalColumn: "BedId");
                    table.ForeignKey(
                        name: "FK_TENANTS_MST_FLAT_MST_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "FLAT_MST",
                        principalColumn: "FlatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TENANTS_MST_ZONE_MST_RoomId",
                        column: x => x.RoomId,
                        principalTable: "ZONE_MST",
                        principalColumn: "ZoneId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TENANTS_MST_BedId",
                table: "TENANTS_MST",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_TENANTS_MST_PropertyId",
                table: "TENANTS_MST",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_TENANTS_MST_RoomId",
                table: "TENANTS_MST",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TENANTS_MST");
        }
    }
}
