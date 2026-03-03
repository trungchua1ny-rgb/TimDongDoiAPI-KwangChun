using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimDongDoi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBanFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BanReason",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BannedUntil",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FromUserId",
                table: "Reviews",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ToUserId",
                table: "Reviews",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_users_FromUserId",
                table: "Reviews",
                column: "FromUserId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_users_ToUserId",
                table: "Reviews",
                column: "ToUserId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_users_FromUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_users_ToUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_FromUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ToUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BanReason",
                table: "users");

            migrationBuilder.DropColumn(
                name: "BannedUntil",
                table: "users");
        }
    }
}
