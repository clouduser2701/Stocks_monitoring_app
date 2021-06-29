using Microsoft.EntityFrameworkCore.Migrations;

namespace StocksApp.Server.Migrations
{
    public partial class stockinfochangeanother : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "T",
                table: "StocksInfo");

            migrationBuilder.AddColumn<string>(
                name: "DateString",
                table: "StocksInfo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateString",
                table: "StocksInfo");

            migrationBuilder.AddColumn<long>(
                name: "T",
                table: "StocksInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
