using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintAndExpenseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "USER_MST",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "USER_MST",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBySuperAdminId",
                table: "USER_MST",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "PgName",
                table: "USER_MST",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "COMPLAINT_MST",
                columns: table => new
                {
                    ComplaintId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FlatId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdminRemark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttachmentName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttachmentUri = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedByUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ComplaintDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPLAINT_MST", x => x.ComplaintId);
                    table.ForeignKey(
                        name: "FK_COMPLAINT_MST_FLAT_MST_FlatId",
                        column: x => x.FlatId,
                        principalTable: "FLAT_MST",
                        principalColumn: "FlatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMPLAINT_MST_USER_MST_TenantId",
                        column: x => x.TenantId,
                        principalTable: "USER_MST",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMPLAINT_MST_USER_MST_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "USER_MST",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EXPENSE_MST",
                columns: table => new
                {
                    ExpenseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FlatId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsCommonExpense = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Month = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentMode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaidBy = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiptName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiptUri = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSE_MST", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_EXPENSE_MST_FLAT_MST_FlatId",
                        column: x => x.FlatId,
                        principalTable: "FLAT_MST",
                        principalColumn: "FlatId");
                    table.ForeignKey(
                        name: "FK_EXPENSE_MST_USER_MST_UserId",
                        column: x => x.UserId,
                        principalTable: "USER_MST",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "USER_MST",
                keyColumn: "UserId",
                keyValue: new Guid("5639d859-f1b8-4a90-bb07-0279f38a580c"),
                columns: new[] { "Address", "City", "CreatedBySuperAdminId", "PgName" },
                values: new object[] { null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_USER_MST_CreatedBySuperAdminId",
                table: "USER_MST",
                column: "CreatedBySuperAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPLAINT_MST_FlatId",
                table: "COMPLAINT_MST",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPLAINT_MST_TenantId",
                table: "COMPLAINT_MST",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPLAINT_MST_UpdatedByUserId",
                table: "COMPLAINT_MST",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_MST_FlatId",
                table: "EXPENSE_MST",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_MST_UserId",
                table: "EXPENSE_MST",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_USER_MST_USER_MST_CreatedBySuperAdminId",
                table: "USER_MST",
                column: "CreatedBySuperAdminId",
                principalTable: "USER_MST",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_USER_MST_USER_MST_CreatedBySuperAdminId",
                table: "USER_MST");

            migrationBuilder.DropTable(
                name: "COMPLAINT_MST");

            migrationBuilder.DropTable(
                name: "EXPENSE_MST");

            migrationBuilder.DropIndex(
                name: "IX_USER_MST_CreatedBySuperAdminId",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "City",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "CreatedBySuperAdminId",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "PgName",
                table: "USER_MST");
        }
    }
}
