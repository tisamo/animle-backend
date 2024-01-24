using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class likesdailygameversus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizes_Users_userId",
                table: "Quizes");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Quizes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizes_userId",
                table: "Quizes",
                newName: "IX_Quizes_UserId");

            migrationBuilder.CreateTable(
                name: "DailyGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimePlayed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LikedEntityType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LikedEntityId = table.Column<int>(type: "int", nullable: false),
                    LikedTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Versus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Points = table.Column<int>(type: "int", nullable: false),
                    gameWon = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Versus_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DailyGames_UserId",
                table: "DailyGames",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Versus_UserId",
                table: "Versus",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizes_Users_UserId",
                table: "Quizes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizes_Users_UserId",
                table: "Quizes");

            migrationBuilder.DropTable(
                name: "DailyGames");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Versus");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Quizes",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizes_UserId",
                table: "Quizes",
                newName: "IX_Quizes_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizes_Users_userId",
                table: "Quizes",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
