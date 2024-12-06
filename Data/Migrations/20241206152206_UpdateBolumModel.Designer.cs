﻿// <auto-generated />
using System;
using DersPrgProjesi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DersPrgProjesi.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241206152206_UpdateBolumModel")]
    partial class UpdateBolumModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DersPrgProjesi.Models.Bolum", b =>
                {
                    b.Property<int>("BolumID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BolumID"));

                    b.Property<string>("BolumAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BolumMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BolumSifre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FakulteID")
                        .HasColumnType("int");

                    b.HasKey("BolumID");

                    b.HasIndex("FakulteID");

                    b.ToTable("Bolumler");
                });

            modelBuilder.Entity("DersPrgProjesi.Models.Fakulte", b =>
                {
                    b.Property<int>("FakulteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FakulteID"));

                    b.Property<string>("FakulteAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FakulteMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FakulteSifre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FakulteID");

                    b.ToTable("Fakulteler");
                });

            modelBuilder.Entity("DersPrgProjesi.Models.admin", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("DersPrgProjesi.Models.Bolum", b =>
                {
                    b.HasOne("DersPrgProjesi.Models.Fakulte", "Fakulte")
                        .WithMany()
                        .HasForeignKey("FakulteID");

                    b.Navigation("Fakulte");
                });
#pragma warning restore 612, 618
        }
    }
}
