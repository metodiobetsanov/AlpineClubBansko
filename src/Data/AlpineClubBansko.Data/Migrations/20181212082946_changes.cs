using Microsoft.EntityFrameworkCore.Migrations;

namespace AlpineClubBansko.Data.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Photos");
        }
    }
}
