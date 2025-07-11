using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Services.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstPlayerUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecondPlayerUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Games_Users_FirstPlayerUuid",
                        column: x => x.FirstPlayerUuid,
                        principalTable: "Users",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Users_SecondPlayerUuid",
                        column: x => x.SecondPlayerUuid,
                        principalTable: "Users",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Users_WinnerUuid",
                        column: x => x.WinnerUuid,
                        principalTable: "Users",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Moves_Games_GameUuid",
                        column: x => x.GameUuid,
                        principalTable: "Games",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Moves_Users_PlayerUuid",
                        column: x => x.PlayerUuid,
                        principalTable: "Users",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_FirstPlayerUuid",
                table: "Games",
                column: "FirstPlayerUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Games_SecondPlayerUuid",
                table: "Games",
                column: "SecondPlayerUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerUuid",
                table: "Games",
                column: "WinnerUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameUuid",
                table: "Moves",
                column: "GameUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PlayerUuid",
                table: "Moves",
                column: "PlayerUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
