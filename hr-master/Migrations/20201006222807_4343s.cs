using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _4343s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Complaint",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Complain = table.Column<string>(nullable: true),
                    InternetUser_Id = table.Column<Guid>(nullable: false),
                    Complain_Date = table.Column<DateTime>(nullable: false),
                    Complain_Status = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Complaint", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Complaint");
        }
    }
}
