using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com_Fi.Data.Migrations
{
    public partial class AddedMusicAlbumList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musics_Albums_AlbumsId",
                table: "Musics");

            migrationBuilder.DropIndex(
                name: "IX_Musics_AlbumsId",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "AlbumsId",
                table: "Musics");

            migrationBuilder.CreateTable(
                name: "AlbumsMusics",
                columns: table => new
                {
                    AlbumMusicsId = table.Column<int>(type: "int", nullable: false),
                    MusicAlbumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumsMusics", x => new { x.AlbumMusicsId, x.MusicAlbumId });
                    table.ForeignKey(
                        name: "FK_AlbumsMusics_Albums_MusicAlbumId",
                        column: x => x.MusicAlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumsMusics_Musics_AlbumMusicsId",
                        column: x => x.AlbumMusicsId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "ade603da-b657-470a-a3c7-ac77526e87fd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "50daed1d-b5f8-4755-9a56-c4cdae532560");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsMusics_MusicAlbumId",
                table: "AlbumsMusics",
                column: "MusicAlbumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumsMusics");

            migrationBuilder.AddColumn<int>(
                name: "AlbumsId",
                table: "Musics",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "206de5b4-60af-4c40-b12d-0fe6e7513798");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "99afa7a9-2db3-4fa7-839c-168ce04e443e");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_AlbumsId",
                table: "Musics",
                column: "AlbumsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_Albums_AlbumsId",
                table: "Musics",
                column: "AlbumsId",
                principalTable: "Albums",
                principalColumn: "Id");
        }
    }
}
