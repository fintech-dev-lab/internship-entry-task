using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDrawProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_WinnerUuid",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraw",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_WinnerUuid",
                table: "Games",
                column: "WinnerUuid",
                principalTable: "Users",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_WinnerUuid",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsDraw",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_WinnerUuid",
                table: "Games",
                column: "WinnerUuid",
                principalTable: "Users",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
