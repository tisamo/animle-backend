using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animle.Migrations
{
    /// <inheritdoc />
    public partial class fingerprints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
             name: "FingerPrint",
             table: "gamecontests",
             type: "nvarchar(100)",
             nullable: true);
            
            migrationBuilder.AddColumn<string>(
             name: "FingerPrint",
             table: "userguessgame",
             type: "nvarchar(100)",
             nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "FingerPrint",
             table: "userguessgame");

            migrationBuilder.DropColumn(
          name: "FingerPrint",
           table: "gamecontests");

        }
    }
}
