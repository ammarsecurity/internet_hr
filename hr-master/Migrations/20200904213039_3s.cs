using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_master.Migrations
{
    public partial class _3s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company_Locition",
                table: "AdminUser",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Delay_penalty",
                table: "AdminUser",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Attachment_Name = table.Column<string>(nullable: true),
                    Attachment_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployessUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Employee_Name = table.Column<string>(nullable: true),
                    Employee_Fullname = table.Column<string>(nullable: true),
                    Employee_Email = table.Column<string>(nullable: true),
                    Employee_Phone = table.Column<string>(nullable: true),
                    Employee_Password = table.Column<string>(nullable: true),
                    Employee_Team = table.Column<Guid>(nullable: false),
                    Employee_Adress = table.Column<string>(nullable: true),
                    Employee_Photo = table.Column<string>(nullable: true),
                    Employee_Saller = table.Column<decimal>(nullable: false),
                    Employee_Note = table.Column<string>(nullable: true),
                    Registration_Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployessUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InOut",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmplyeeId = table.Column<Guid>(nullable: false),
                    In_Out = table.Column<int>(nullable: false),
                    In_Out_Date = table.Column<DateTime>(nullable: false),
                    In_Out_Time = table.Column<TimeSpan>(nullable: false),
                    In_Out_Status = table.Column<int>(nullable: false),
                    In_Out_Locition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InOut", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Employees_Id = table.Column<Guid>(nullable: false),
                    Penalties_Price = table.Column<decimal>(nullable: false),
                    Penalties_Date = table.Column<DateTime>(nullable: false),
                    Penalties_Note = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stored",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Item_Name = table.Column<string>(nullable: true),
                    Item_Model = table.Column<string>(nullable: true),
                    Item_IsUsed = table.Column<bool>(nullable: false),
                    Item_SerialNumber = table.Column<string>(nullable: true),
                    Item_Count = table.Column<int>(nullable: false),
                    Item_EnteryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stored", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PartName = table.Column<string>(nullable: true),
                    PartNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Task_Title = table.Column<string>(nullable: true),
                    Task_Price = table.Column<decimal>(nullable: false),
                    Task_part = table.Column<Guid>(nullable: false),
                    Task_Note = table.Column<string>(nullable: true),
                    Task_closed_Note = table.Column<string>(nullable: true),
                    Task_Employee_WorkOn = table.Column<Guid>(nullable: false),
                    Task_Employee_Open = table.Column<Guid>(nullable: false),
                    Task_Employee_Close = table.Column<Guid>(nullable: false),
                    Task_Date = table.Column<DateTime>(nullable: false),
                    Task_EndDate = table.Column<DateTime>(nullable: false),
                    Task_Open = table.Column<DateTime>(nullable: false),
                    Task_Done = table.Column<DateTime>(nullable: false),
                    Tower_Id = table.Column<Guid>(nullable: false),
                    Task_Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    Team_Name = table.Column<string>(nullable: true),
                    Team_Roles = table.Column<int>(nullable: false),
                    Team_Note = table.Column<string>(nullable: true),
                    In_Time = table.Column<TimeSpan>(nullable: false),
                    Out_Time = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Towers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tower_Name = table.Column<string>(nullable: true),
                    Tower_Ip = table.Column<string>(nullable: true),
                    Tower_locition = table.Column<string>(nullable: true),
                    Tower_Owner = table.Column<string>(nullable: true),
                    Tower_Owner_Number = table.Column<string>(nullable: true),
                    Tower_Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "EmployessUsers");

            migrationBuilder.DropTable(
                name: "InOut");

            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "Stored");

            migrationBuilder.DropTable(
                name: "StoredParts");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Towers");

            migrationBuilder.DropColumn(
                name: "Company_Locition",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Delay_penalty",
                table: "AdminUser");
        }
    }
}
