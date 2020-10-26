using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _57s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreMovements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Movment_Employee = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Movement_date = table.Column<DateTime>(nullable: false),
                    Movement_type = table.Column<int>(nullable: false),
                    Movement_Count = table.Column<int>(nullable: false),
                    Movement_Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMovements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMovements");
        }
    }
}
