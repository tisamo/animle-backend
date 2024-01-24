using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class threebythreetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeThreebythreecs_AnimeWithEmoji_AnimesId",
                table: "AnimeThreebythreecs");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreeId",
                table: "AnimeThreebythreecs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeThreebythreecs",
                table: "AnimeThreebythreecs");

            migrationBuilder.RenameTable(
                name: "AnimeThreebythreecs",
                newName: "AnimeThreebythree");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeThreebythreecs_ThreebythreeId",
                table: "AnimeThreebythree",
                newName: "IX_AnimeThreebythree_ThreebythreeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeThreebythree",
                table: "AnimeThreebythree",
                columns: new[] { "AnimesId", "ThreebythreeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeThreebythree_AnimeWithEmoji_AnimesId",
                table: "AnimeThreebythree",
                column: "AnimesId",
                principalTable: "AnimeWithEmoji",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeThreebythree_Threebythree_ThreebythreeId",
                table: "AnimeThreebythree",
                column: "ThreebythreeId",
                principalTable: "Threebythree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeThreebythree_AnimeWithEmoji_AnimesId",
                table: "AnimeThreebythree");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeThreebythree_Threebythree_ThreebythreeId",
                table: "AnimeThreebythree");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeThreebythree",
                table: "AnimeThreebythree");

            migrationBuilder.RenameTable(
                name: "AnimeThreebythree",
                newName: "AnimeThreebythreecs");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeThreebythree_ThreebythreeId",
                table: "AnimeThreebythreecs",
                newName: "IX_AnimeThreebythreecs_ThreebythreeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeThreebythreecs",
                table: "AnimeThreebythreecs",
                columns: new[] { "AnimesId", "ThreebythreeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeThreebythreecs_AnimeWithEmoji_AnimesId",
                table: "AnimeThreebythreecs",
                column: "AnimesId",
                principalTable: "AnimeWithEmoji",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeThreebythreecs_Threebythree_ThreebythreeId",
                table: "AnimeThreebythreecs",
                column: "ThreebythreeId",
                principalTable: "Threebythree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
