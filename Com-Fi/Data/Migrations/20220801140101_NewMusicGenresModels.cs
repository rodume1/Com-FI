using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com_Fi.Data.Migrations
{
    public partial class NewMusicGenresModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenresMusics");

            migrationBuilder.AddColumn<int>(
                name: "GenreFK",
                table: "Musics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Musics",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "7a68bfa5-50d1-45ff-92b6-6bdc45aa9c6d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "65ccddbc-4425-404d-8a42-b3c6f21a8bc7");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_GenreId",
                table: "Musics",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_Genres_GenreId",
                table: "Musics",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musics_Genres_GenreId",
                table: "Musics");

            migrationBuilder.DropIndex(
                name: "IX_Musics_GenreId",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "GenreFK",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Musics");

            migrationBuilder.CreateTable(
                name: "GenresMusics",
                columns: table => new
                {
                    MusicGenresId = table.Column<int>(type: "int", nullable: false),
                    MusicGenresId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenresMusics", x => new { x.MusicGenresId, x.MusicGenresId1 });
                    table.ForeignKey(
                        name: "FK_GenresMusics_Genres_MusicGenresId",
                        column: x => x.MusicGenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenresMusics_Musics_MusicGenresId1",
                        column: x => x.MusicGenresId1,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "55039a06-3e81-40b8-a971-2353cb4db6d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "u",
                column: "ConcurrencyStamp",
                value: "8d55d3a5-2cdf-4999-b2e9-36f440432728");

            migrationBuilder.CreateIndex(
                name: "IX_GenresMusics_MusicGenresId1",
                table: "GenresMusics",
                column: "MusicGenresId1");
        }
    }
}
