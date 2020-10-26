using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _17s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company_Locition",
                table: "AdminUser");

            migrationBuilder.AddColumn<string>(
                name: "Company_Latitude",
                table: "AdminUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company_Longitude",
                table: "AdminUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company_Latitude",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Company_Longitude",
                table: "AdminUser");

            migrationBuilder.AddColumn<string>(
                name: "Company_Locition",
                table: "AdminUser",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
