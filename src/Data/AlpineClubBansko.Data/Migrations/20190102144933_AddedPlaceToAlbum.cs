using Microsoft.EntityFrameworkCore.Migrations;

namespace AlpineClubBansko.Data.Migrations
{
    public partial class AddedPlaceToAlbum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Albums",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Albums");
        }
    }
}