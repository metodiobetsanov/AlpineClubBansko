using Microsoft.EntityFrameworkCore.Migrations;

namespace AlpineClubBansko.Data.Migrations
{
    public partial class R : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteId",
                table: "Stories",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Albums",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Stories_RouteId",
                table: "Stories",
                column: "RouteId",
                unique: true,
                filter: "[RouteId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Routes_RouteId",
                table: "Stories",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Routes_RouteId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Stories_RouteId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Stories");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Albums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}