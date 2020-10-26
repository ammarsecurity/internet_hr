using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _56s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee_Time",
                table: "EmployessUsers");

            migrationBuilder.AddColumn<string>(
                name: "Employee_In_Time",
                table: "EmployessUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee_In_Time",
                table: "EmployessUsers");

            migrationBuilder.AddColumn<string>(
                name: "Employee_Time",
                table: "EmployessUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
