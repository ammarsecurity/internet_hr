using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _223232s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    User_FullName = table.Column<string>(nullable: true),
                    User_Name = table.Column<string>(nullable: true),
                    User_Password = table.Column<string>(nullable: true),
                    User_Card = table.Column<string>(nullable: true),
                    User_Price = table.Column<string>(nullable: true),
                    User_ActiveDate = table.Column<DateTime>(nullable: false),
                    User_EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternetUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternetUsers");
        }
    }
}
