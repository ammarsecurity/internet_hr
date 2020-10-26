using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _33222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InternetUserId",
                table: "Tasks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Tower_Broadcasting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tower_Id = table.Column<Guid>(nullable: false),
                    Broadcasting_SSID = table.Column<string>(nullable: true),
                    Employee_Id = table.Column<Guid>(nullable: false),
                    Broadcasting_Type = table.Column<string>(nullable: true),
                    Broadcasting_UserName = table.Column<string>(nullable: true),
                    Broadcasting_Password = table.Column<string>(nullable: true),
                    Broadcasting_SerailNamber = table.Column<string>(nullable: true),
                    Broadcasting_Time = table.Column<DateTime>(nullable: false),
                    Broadcasting_Ip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tower_Broadcasting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tower_Electrical",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tower_Id = table.Column<Guid>(nullable: false),
                    Electrical_Name = table.Column<string>(nullable: true),
                    Employee_Id = table.Column<Guid>(nullable: false),
                    Electrical_Type = table.Column<string>(nullable: true),
                    Electrical_SerailNamber = table.Column<string>(nullable: true),
                    Electrical_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tower_Electrical", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tower_Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tower_Id = table.Column<Guid>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Notes_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tower_Notes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tower_Broadcasting");

            migrationBuilder.DropTable(
                name: "Tower_Electrical");

            migrationBuilder.DropTable(
                name: "Tower_Notes");

            migrationBuilder.DropColumn(
                name: "InternetUserId",
                table: "Tasks");
        }
    }
}
