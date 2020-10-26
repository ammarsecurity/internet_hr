using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _55s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Employee_Out_Time",
                table: "EmployessUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Employee_Time",
                table: "EmployessUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee_Out_Time",
                table: "EmployessUsers");

            migrationBuilder.DropColumn(
                name: "Employee_Time",
                table: "EmployessUsers");
        }
    }
}
