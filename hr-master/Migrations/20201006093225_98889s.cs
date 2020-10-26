using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _98889s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User_Adress",
                table: "InternetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Tower",
                table: "InternetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User_Adress",
                table: "InternetUsers");

            migrationBuilder.DropColumn(
                name: "User_Tower",
                table: "InternetUsers");
        }
    }
}
