using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _1111111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OpentTask_part",
                table: "Tasks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpentTask_part",
                table: "Tasks");
        }
    }
}
