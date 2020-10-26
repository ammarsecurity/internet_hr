using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _9s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Towers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Teams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Tasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "StoredParts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Stored",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Penalties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "EmployessUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Attachment",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Towers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "StoredParts");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Stored");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "EmployessUsers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Attachment");
        }
    }
}
