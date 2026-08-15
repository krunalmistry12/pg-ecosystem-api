using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateqqComplaintAndExpenseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COMPLAINT_MST_USER_MST_TenantId",
                table: "COMPLAINT_MST");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                table: "COMPLAINT_MST",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_COMPLAINT_MST_TENANTS_MST_TenantId",
                table: "COMPLAINT_MST",
                column: "TenantId",
                principalTable: "TENANTS_MST",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COMPLAINT_MST_TENANTS_MST_TenantId",
                table: "COMPLAINT_MST");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "COMPLAINT_MST",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_COMPLAINT_MST_USER_MST_TenantId",
                table: "COMPLAINT_MST",
                column: "TenantId",
                principalTable: "USER_MST",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
