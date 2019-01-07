using Microsoft.EntityFrameworkCore.Migrations;

namespace AlpineClubBansko.Data.Migrations
{
    public partial class DecimalPrecision2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                type: "decimal(11, 8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Locations",
                type: "decimal(11,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                type: "decimal(8, 3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Locations",
                type: "decimal(8, 3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,8)");
        }
    }
}