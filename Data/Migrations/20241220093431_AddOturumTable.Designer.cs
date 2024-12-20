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
    [Migration("20241220093431_AddOturumTable")]
    partial class AddOturumTable
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

            modelBuilder.Entity("DersPrgProjesi.Models.Oturum", b =>
                {
                    b.Property<int>("OturumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OturumId"));

                    b.Property<TimeSpan>("BaslangicSaati")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("BitisSaati")
                        .HasColumnType("time");

                    b.Property<int>("Gun")
                        .HasColumnType("int");

                    b.Property<int?>("SınıfID")
                        .HasColumnType("int");

                    b.HasKey("OturumId");

                    b.HasIndex("SınıfID");

                    b.ToTable("Oturumlar");
                });

            modelBuilder.Entity("DersPrgProjesi.Models.Sınıf", b =>
                {
                    b.Property<int>("SınıfID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SınıfID"));

                    b.Property<int?>("FakulteID")
                        .HasColumnType("int");

                    b.Property<int>("Kapasite")
                        .HasColumnType("int");

                    b.Property<int>("SınavKapasite")
                        .HasColumnType("int");

                    b.Property<string>("SınıfAd")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SınıfID");

                    b.HasIndex("FakulteID");

                    b.ToTable("Sınıflar");
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

            modelBuilder.Entity("DersPrgProjesi.Models.Oturum", b =>
                {
                    b.HasOne("DersPrgProjesi.Models.Sınıf", "Sınıf")
                        .WithMany()
                        .HasForeignKey("SınıfID");

                    b.Navigation("Sınıf");
                });

            modelBuilder.Entity("DersPrgProjesi.Models.Sınıf", b =>
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