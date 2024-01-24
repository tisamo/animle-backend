using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

     

         

        

            migrationBuilder.CreateTable(
                name: "Threebythree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threebythree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threebythree_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");


            migrationBuilder.CreateTable(
                name: "AnimeThreebythreecs",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    ThreebythreecsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeThreebythreecs", x => new { x.AnimesId, x.ThreebythreecsId });
                    table.ForeignKey(
                        name: "FK_AnimeThreebythreecs_AnimeWithEmoji_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "AnimeWithEmoji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreecsId",
                        column: x => x.ThreebythreecsId,
                        principalTable: "Threebythree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeThreebythreecs_ThreebythreecsId",
                table: "AnimeThreebythreecs",
                column: "ThreebythreecsId");


            migrationBuilder.CreateIndex(
                name: "IX_Threebythree_UserId",
                table: "Threebythree",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeThreebythreecs");


            migrationBuilder.DropTable(
                name: "Threebythree");

  
        }
    }
}
