using Microsoft.EntityFrameworkCore.Migrations;

namespace StocksApp.Server.Migrations
{
    public partial class stockinfochange6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "V",
                table: "StocksInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "V",
                table: "StocksInfo",
                type: "decimal(10,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
