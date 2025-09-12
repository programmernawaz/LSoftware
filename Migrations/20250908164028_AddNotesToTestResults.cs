using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathLabAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToTestResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "TestResults");
        }
    }
}
