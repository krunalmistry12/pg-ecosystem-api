using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantRentUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RENTS_MST",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoomId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BillingMonth = table.Column<int>(type: "int", nullable: false),
                    BillingYear = table.Column<int>(type: "int", nullable: false),
                    BaseRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ElectricityBill = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtraCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LateFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    StartingMeterReading = table.Column<double>(type: "double", nullable: true),
                    EndingMeterReading = table.Column<double>(type: "double", nullable: true),
                    UnitsConsumed = table.Column<double>(type: "double", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENTS_MST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RENTS_MST_BED_MST_BedId",
                        column: x => x.BedId,
                        principalTable: "BED_MST",
                        principalColumn: "BedId");
                    table.ForeignKey(
                        name: "FK_RENTS_MST_FLAT_MST_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "FLAT_MST",
                        principalColumn: "FlatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RENTS_MST_TENANTS_MST_TenantId",
                        column: x => x.TenantId,
                        principalTable: "TENANTS_MST",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RENTS_MST_ZONE_MST_RoomId",
                        column: x => x.RoomId,
                        principalTable: "ZONE_MST",
                        principalColumn: "ZoneId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RENT_PAYMENTS_TRN",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReceiptNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RentId = table.Column<long>(type: "bigint", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentMode = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    TransactionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remarks = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENT_PAYMENTS_TRN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RENT_PAYMENTS_TRN_RENTS_MST_RentId",
                        column: x => x.RentId,
                        principalTable: "RENTS_MST",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RENT_PAYMENTS_TRN_RentId",
                table: "RENT_PAYMENTS_TRN",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_RENTS_MST_BedId",
                table: "RENTS_MST",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_RENTS_MST_PropertyId",
                table: "RENTS_MST",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RENTS_MST_RoomId",
                table: "RENTS_MST",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RENTS_MST_TenantId_BillingMonth_BillingYear",
                table: "RENTS_MST",
                columns: new[] { "TenantId", "BillingMonth", "BillingYear" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RENT_PAYMENTS_TRN");

            migrationBuilder.DropTable(
                name: "RENTS_MST");
        }
    }
}
