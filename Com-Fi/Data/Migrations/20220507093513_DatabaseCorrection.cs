using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com_Fi.Data.Migrations
{
    public partial class DatabaseCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artists_ArtistsId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ArtistsId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "ArtistsId",
                table: "Albums");

            migrationBuilder.CreateTable(
                name: "AlbumsArtists",
                columns: table => new
                {
                    AlbumArtistsId = table.Column<int>(type: "int", nullable: false),
                    AlbumArtistsId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumsArtists", x => new { x.AlbumArtistsId, x.AlbumArtistsId1 });
                    table.ForeignKey(
                        name: "FK_AlbumsArtists_Albums_AlbumArtistsId",
                        column: x => x.AlbumArtistsId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumsArtists_Artists_AlbumArtistsId1",
                        column: x => x.AlbumArtistsId1,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsArtists_AlbumArtistsId1",
                table: "AlbumsArtists",
                column: "AlbumArtistsId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumsArtists");

            migrationBuilder.AddColumn<int>(
                name: "ArtistsId",
                table: "Albums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistsId",
                table: "Albums",
                column: "ArtistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artists_ArtistsId",
                table: "Albums",
                column: "ArtistsId",
                principalTable: "Artists",
                principalColumn: "Id");
        }
    }
}
