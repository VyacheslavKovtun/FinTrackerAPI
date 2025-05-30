using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrackerAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailConfirmationTokenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "Users");
        }
    }
}
