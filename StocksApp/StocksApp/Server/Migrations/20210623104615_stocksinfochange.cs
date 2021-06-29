using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StocksApp.Server.Migrations
{
    public partial class stocksinfochange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDate",
                table: "StocksInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDate",
                table: "StocksInfo");
        }
    }
}
