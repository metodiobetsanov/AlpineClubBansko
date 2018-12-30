using Microsoft.EntityFrameworkCore.Migrations;

namespace AlpineClubBansko.Data.Migrations
{
    public partial class favorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Stories");

            migrationBuilder.RenameColumn(
                name: "Viewed",
                table: "Stories",
                newName: "Views");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Views",
                table: "Stories",
                newName: "Viewed");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Stories",
                nullable: false,
                defaultValue: 0);
        }
    }
}
