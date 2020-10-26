using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _58s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Task_Price",
                table: "Tasks");

            migrationBuilder.AddColumn<Guid>(
                name: "Task_Price_rewards",
                table: "Tasks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Task_Price_rewards",
                table: "Tasks");

            migrationBuilder.AddColumn<decimal>(
                name: "Task_Price",
                table: "Tasks",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
