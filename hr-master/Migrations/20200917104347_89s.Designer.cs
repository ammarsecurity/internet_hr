﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hr_master.Db;

namespace hr_master.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200917104347_89s")]
    partial class _89s
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("hr_master.Models.AdminUsers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Company_Latitude")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Company_Longitude")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Delay_penalty")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("Registration_Data")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("User_Firstname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User_Level")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User_Mail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User_Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User_Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AdminUser");
                });

            modelBuilder.Entity("hr_master.Models.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Attachment_Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Attachment_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("hr_master.Models.EmployessUsers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Employee_Adress")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Fullname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_In_Time")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Out_Time")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Photo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Employee_Saller")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("Employee_Team")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Registration_Data")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("EmployessUsers");
                });

            modelBuilder.Entity("hr_master.Models.InOut", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Employee_Latitude")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Employee_Longitude")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("EmplyeeId")
                        .HasColumnType("char(36)");

                    b.Property<int>("In_Out")
                        .HasColumnType("int");

                    b.Property<DateTime>("In_Out_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("In_Out_Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("In_Out_Time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("distance")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("InOut");
                });

            modelBuilder.Entity("hr_master.Models.Penalties", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Employees_Id")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Penalties_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("Penalties_Enterid")
                        .HasColumnType("char(36)");

                    b.Property<string>("Penalties_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Penalties_Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Penalties");
                });

            modelBuilder.Entity("hr_master.Models.RewardsTable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("RewardsInfo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("RewardsPrice")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("RewardsTable");
                });

            modelBuilder.Entity("hr_master.Models.ScheduleDelayPenalties", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("PenaltiesPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("formtime")
                        .HasColumnType("int");

                    b.Property<int>("totime")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ScheduleDelayPenalties");
                });

            modelBuilder.Entity("hr_master.Models.StoreMovements", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Movement_Count")
                        .HasColumnType("int");

                    b.Property<string>("Movement_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Movement_Received")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Movement_date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Movement_type")
                        .HasColumnType("int");

                    b.Property<Guid>("Movment_Employee")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("StoreMovements");
                });

            modelBuilder.Entity("hr_master.Models.Stored", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Item_Count")
                        .HasColumnType("int");

                    b.Property<Guid>("Item_EmployeeEntery")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Item_EnteryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Item_IsUsed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Item_Model")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Item_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("Item_Part")
                        .HasColumnType("char(36)");

                    b.Property<string>("Item_SerialNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Stored");
                });

            modelBuilder.Entity("hr_master.Models.StoredParts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PartName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PartNote")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("StoredParts");
                });

            modelBuilder.Entity("hr_master.Models.Tasks", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Task_Date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Task_Done")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("Task_Employee_Close")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Task_Employee_Open")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Task_Employee_WorkOn")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Task_EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Task_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Task_Open")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("Task_Price_rewards")
                        .HasColumnType("char(36)");

                    b.Property<int>("Task_Status")
                        .HasColumnType("int");

                    b.Property<string>("Task_Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Task_closed_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("Task_part")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Tower_Id")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("hr_master.Models.TasksRepalys", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("RepalyDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Repaly_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("TasksRepalys");
                });

            modelBuilder.Entity("hr_master.Models.Teams", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("In_Time")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Out_Time")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Team_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Team_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Team_Roles")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("hr_master.Models.Towers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Tower_Ip")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Tower_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Tower_Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Tower_Owner")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Tower_Owner_Number")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Tower_locition")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Towers");
                });
#pragma warning restore 612, 618
        }
    }
}
