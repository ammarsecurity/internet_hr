using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _89ss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "In_Out_Locition",
                table: "InOut");

            migrationBuilder.AddColumn<string>(
                name: "Employee_Latitude",
                table: "InOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Employee_Longitude",
                table: "InOut",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee_Latitude",
                table: "InOut");

            migrationBuilder.DropColumn(
                name: "Employee_Longitude",
                table: "InOut");

            migrationBuilder.AddColumn<string>(
                name: "In_Out_Locition",
                table: "InOut",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
