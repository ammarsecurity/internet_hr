using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _44s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleDelayPenalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    formtime = table.Column<int>(nullable: false),
                    totime = table.Column<int>(nullable: false),
                    PenaltiesPrice = table.Column<decimal>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDelayPenalties", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleDelayPenalties");
        }
    }
}
