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
    [Migration("20200903210443_2s")]
    partial class _2s
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
#pragma warning restore 612, 618
        }
    }
}
