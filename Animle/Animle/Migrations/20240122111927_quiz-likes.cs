using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class quizlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreecsId",
                table: "AnimeThreebythreecs");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.RenameColumn(
                name: "ThreebythreecsId",
                table: "AnimeThreebythreecs",
                newName: "ThreebythreeId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeThreebythreecs_ThreebythreecsId",
                table: "AnimeThreebythreecs",
                newName: "IX_AnimeThreebythreecs_ThreebythreeId");

            migrationBuilder.CreateTable(
                name: "QuizLikes",
                columns: table => new
                {
                    quizId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizLikes", x => new { x.UserId, x.quizId });
                    table.ForeignKey(
                        name: "FK_QuizLikes_Quizes_quizId",
                        column: x => x.quizId,
                        principalTable: "Quizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "threebythreeLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ThreebythreeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_threebythreeLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_threebythreeLikes_Threebythree_ThreebythreeId",
                        column: x => x.ThreebythreeId,
                        principalTable: "Threebythree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_threebythreeLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_QuizLikes_quizId",
                table: "QuizLikes",
                column: "quizId");

            migrationBuilder.CreateIndex(
                name: "IX_threebythreeLikes_ThreebythreeId",
                table: "threebythreeLikes",
                column: "ThreebythreeId");

            migrationBuilder.CreateIndex(
                name: "IX_threebythreeLikes_UserId",
                table: "threebythreeLikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreeId",
                table: "AnimeThreebythreecs",
                column: "ThreebythreeId",
                principalTable: "Threebythree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreeId",
                table: "AnimeThreebythreecs");

            migrationBuilder.DropTable(
                name: "QuizLikes");

            migrationBuilder.DropTable(
                name: "threebythreeLikes");

            migrationBuilder.RenameColumn(
                name: "ThreebythreeId",
                table: "AnimeThreebythreecs",
                newName: "ThreebythreecsId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeThreebythreecs_ThreebythreeId",
                table: "AnimeThreebythreecs",
                newName: "IX_AnimeThreebythreecs_ThreebythreecsId");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LikedEntityId = table.Column<int>(type: "int", nullable: false),
                    LikedEntityType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreecsId",
                table: "AnimeThreebythreecs",
                column: "ThreebythreecsId",
                principalTable: "Threebythree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
