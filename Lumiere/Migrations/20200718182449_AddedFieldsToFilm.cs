using Microsoft.EntityFrameworkCore.Migrations;

namespace Lumiere.Migrations
{
    public partial class AddedFieldsToFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Seances",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "Films",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KinopoiskId",
                table: "Films",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "Seances");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "KinopoiskId",
                table: "Films");
        }
    }
}
