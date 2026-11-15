using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeCa.Migrations
{
    /// <inheritdoc />
    public partial class beaber1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BearerTokenExpiresAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BearerTokenHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BearerTokenExpiresAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BearerTokenHash",
                table: "Users");
        }
    }
}
