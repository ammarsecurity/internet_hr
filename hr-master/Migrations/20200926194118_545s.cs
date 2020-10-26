using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _545s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "InputAndOutput_Price",
                table: "AccounterInputAndOutput",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InputAndOutput_Status",
                table: "AccounterInputAndOutput",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputAndOutput_Status",
                table: "AccounterInputAndOutput");

            migrationBuilder.AlterColumn<int>(
                name: "InputAndOutput_Price",
                table: "AccounterInputAndOutput",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
