﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sales.EfContext;


#nullable disable

namespace Sales.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230214164856_SeedDeps")]
    partial class SeedDeps
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sales.EFContext.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d3c376e4-bce3-4d85-aba4-e3cf49612c94"),
                            Name = "IT отдел"
                        },
                        new
                        {
                            Id = new Guid("131ef84b-f06e-494b-848f-bb4bc0604266"),
                            Name = "Бухгалтерия"
                        },
                        new
                        {
                            Id = new Guid("8dcc3969-1d93-47a9-8b79-a30c738db9b4"),
                            Name = "Служба безопасности"
                        },
                        new
                        {
                            Id = new Guid("d2469412-0e4b-46f7-80ec-8c522364d099"),
                            Name = "Отдел кадров"
                        },
                        new
                        {
                            Id = new Guid("1ef7268c-43a8-488c-b761-90982b31df4e"),
                            Name = "Канцелярия"
                        },
                        new
                        {
                            Id = new Guid("415b36d9-2d82-4a92-a313-48312f8e18c6"),
                            Name = "Отдел продаж"
                        },
                        new
                        {
                            Id = new Guid("624b3bb5-0f2c-42b6-a416-099aab799546"),
                            Name = "Юридическая служба"
                        });
                });

            modelBuilder.Entity("Sales.EFContext.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
