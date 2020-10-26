using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _60s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Movement_Receved",
                table: "StoreMovements");

            migrationBuilder.AddColumn<string>(
                name: "Movement_Received",
                table: "StoreMovements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Movement_Received",
                table: "StoreMovements");

            migrationBuilder.AddColumn<string>(
                name: "Movement_Receved",
                table: "StoreMovements",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
