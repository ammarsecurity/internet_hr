using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _332221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tower_Notes",
                table: "Tower_Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tower_Electrical",
                table: "Tower_Electrical");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tower_Broadcasting",
                table: "Tower_Broadcasting");

            migrationBuilder.RenameTable(
                name: "Tower_Notes",
                newName: "TowerNotes");

            migrationBuilder.RenameTable(
                name: "Tower_Electrical",
                newName: "TowerElectrical");

            migrationBuilder.RenameTable(
                name: "Tower_Broadcasting",
                newName: "TowerBroadcasting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TowerNotes",
                table: "TowerNotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TowerElectrical",
                table: "TowerElectrical",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TowerBroadcasting",
                table: "TowerBroadcasting",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TowerNotes",
                table: "TowerNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TowerElectrical",
                table: "TowerElectrical");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TowerBroadcasting",
                table: "TowerBroadcasting");

            migrationBuilder.RenameTable(
                name: "TowerNotes",
                newName: "Tower_Notes");

            migrationBuilder.RenameTable(
                name: "TowerElectrical",
                newName: "Tower_Electrical");

            migrationBuilder.RenameTable(
                name: "TowerBroadcasting",
                newName: "Tower_Broadcasting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tower_Notes",
                table: "Tower_Notes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tower_Electrical",
                table: "Tower_Electrical",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tower_Broadcasting",
                table: "Tower_Broadcasting",
                column: "Id");
        }
    }
}
