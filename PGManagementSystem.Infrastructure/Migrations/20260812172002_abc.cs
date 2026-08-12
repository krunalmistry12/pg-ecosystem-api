using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PGManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class abc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetPg",
                table: "NOTICE_MST");

            migrationBuilder.AddColumn<Guid>(
                name: "FlatId",
                table: "NOTICE_MST",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "NOTICE_MST");

            migrationBuilder.AddColumn<string>(
                name: "TargetPg",
                table: "NOTICE_MST",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
