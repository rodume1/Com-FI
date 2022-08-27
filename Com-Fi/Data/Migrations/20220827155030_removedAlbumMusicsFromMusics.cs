using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com_Fi.Data.Migrations
{
    public partial class removedAlbumMusicsFromMusics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                    AlbumMusicsId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumsMusics", x => new { x.AlbumMusicsId, x.AlbumMusicsId1 });
                    table.ForeignKey(
                        name: "FK_AlbumsMusics_Albums_AlbumMusicsId",
                        column: x => x.AlbumMusicsId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumsMusics_Musics_AlbumMusicsId1",
                        column: x => x.AlbumMusicsId1,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "9b2c68cd-33dd-48ae-b3a8-b2068adaf316");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "d8e50d93-ec2b-4f4c-ab04-46cd0b32503e");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsMusics_AlbumMusicsId1",
                table: "AlbumsMusics",
                column: "AlbumMusicsId1");
        }
    }
}
