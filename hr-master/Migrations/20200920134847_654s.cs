using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _654s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverTimeRewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Employees_Id = table.Column<Guid>(nullable: false),
                    OverTimeRewards_Price = table.Column<decimal>(nullable: false),
                    OverTimeRewards_Date = table.Column<DateTime>(nullable: false),
                    OverTimeRewards_Note = table.Column<string>(nullable: true),
                    OverTimeRewards_Enterid = table.Column<Guid>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverTimeRewards", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverTimeRewards");
        }
    }
}
