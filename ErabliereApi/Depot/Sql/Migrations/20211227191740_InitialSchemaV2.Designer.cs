﻿// <auto-generated />
using System;
using ErabliereApi.Depot.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Depot.Sql.Migrations
{
    [DbContext(typeof(ErabliereDbContext))]
    [Migration("20211227191740_InitialSchemaV2")]
    partial class InitialSchemaV2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ErabliereApi.Donnees.Alerte", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EnvoyerA")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NiveauBassinThresholdHight")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NiveauBassinThresholdLow")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TemperatureThresholdHight")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TemperatureThresholdLow")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("VacciumThresholdHight")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("VacciumThresholdLow")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Alertes");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Baril", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DF")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Q")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("QE")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("IdErabliere");

                    b.ToTable("Barils");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Capteur", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("AfficherCapteurDashboard")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("DC")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Symbole")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("IdErabliere");

                    b.ToTable("Capteurs");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Dompeux", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DD")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DF")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("T")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("IdErabliere");

                    b.ToTable("Dompeux");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Donnee", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("D")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Iddp")
                        .HasColumnType("uniqueidentifier");

                    b.Property<short?>("NB")
                        .HasColumnType("smallint");

                    b.Property<int>("Nboc")
                        .HasColumnType("int");

                    b.Property<int?>("PI")
                        .HasColumnType("int");

                    b.Property<short?>("T")
                        .HasColumnType("smallint");

                    b.Property<short?>("V")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("IdErabliere");

                    b.ToTable("Donnees");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.DonneeCapteur", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("D")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("IdCapteur")
                        .HasColumnType("uniqueidentifier");

                    b.Property<short?>("Valeur")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("IdCapteur");

                    b.ToTable("DonneesCapteur");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Erabliere", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("AfficherSectionBaril")
                        .HasColumnType("bit");

                    b.Property<bool?>("AfficherSectionDompeux")
                        .HasColumnType("bit");

                    b.Property<bool?>("AfficherTrioDonnees")
                        .HasColumnType("bit");

                    b.Property<int?>("IndiceOrdre")
                        .HasColumnType("int");

                    b.Property<string>("IpRule")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Baril", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Barils")
                        .HasForeignKey("IdErabliere");

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Capteur", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Capteurs")
                        .HasForeignKey("IdErabliere");

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Dompeux", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Dompeux")
                        .HasForeignKey("IdErabliere");

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Donnee", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Donnees")
                        .HasForeignKey("IdErabliere");

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.DonneeCapteur", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Capteur", "Capteur")
                        .WithMany("DonneesCapteur")
                        .HasForeignKey("IdCapteur");

                    b.Navigation("Capteur");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Capteur", b =>
                {
                    b.Navigation("DonneesCapteur");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Erabliere", b =>
                {
                    b.Navigation("Barils");

                    b.Navigation("Capteurs");

                    b.Navigation("Dompeux");

                    b.Navigation("Donnees");
                });
#pragma warning restore 612, 618
        }
    }
}
