using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeCa.Migrations
{
    /// <inheritdoc />
    public partial class one434 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BearerTokenExpiresAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BearerTokenHash",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "LoginTokenHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LoginTokenHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
