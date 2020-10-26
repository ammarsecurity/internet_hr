using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _21s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "TasksRepalys",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "TasksRepalys");
        }
    }
}
