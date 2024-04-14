﻿// <auto-generated />
using System;
using CardService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace cardservice.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CardService.Entities.CarPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Package")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CarPages");
                });

            modelBuilder.Entity("CardService.Entities.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarPageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Package")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}