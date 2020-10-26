using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _4433s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccounterInputAndOutput",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InputAndOutput_Price = table.Column<int>(nullable: false),
                    InputAndOutput_Door = table.Column<Guid>(nullable: false),
                    InputAndOutput_Date = table.Column<DateTime>(nullable: false),
                    InputAndOutput_Note = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccounterInputAndOutput", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountersDoor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccounDoor_Name = table.Column<string>(nullable: true),
                    AccounDoor_Status = table.Column<int>(nullable: false),
                    AccounDoor_Info = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountersDoor", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccounterInputAndOutput");

            migrationBuilder.DropTable(
                name: "AccountersDoor");
        }
    }
}
