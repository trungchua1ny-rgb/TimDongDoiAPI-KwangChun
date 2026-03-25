using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimDongDoi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddManualScoresColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManualScores",
                table: "ApplicationTests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManualScores",
                table: "ApplicationTests");
        }
    }
}
