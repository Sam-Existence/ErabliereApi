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
    [Migration("20240220011531_AddIndiceOrdreOnCapteurs")]
    partial class AddIndiceOrdreOnCapteurs
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Conversation", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastMessageDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });

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

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<string>("NiveauBassinThresholdHight")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NiveauBassinThresholdLow")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nom")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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

                    b.HasIndex("IdErabliere");

                    b.ToTable("Alertes");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.AlerteCapteur", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DC")
                        .HasColumnType("datetime2");

                    b.Property<string>("EnvoyerA")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("IdCapteur")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<short?>("MaxValue")
                        .HasColumnType("smallint");

                    b.Property<short?>("MinVaue")
                        .HasColumnType("smallint");

                    b.Property<string>("Nom")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdCapteur");

                    b.ToTable("AlerteCapteurs");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.ApiKey", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeletionTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("RevocationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SubscriptionId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ApiKeys");
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

                    b.Property<bool>("AfficherCapteurDashboard")
                        .HasColumnType("bit");

                    b.Property<bool>("AjouterDonneeDepuisInterface")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("DC")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("IndiceOrdre")
                        .HasColumnType("int");

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

            modelBuilder.Entity("ErabliereApi.Donnees.Customer", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ExternalAccountUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondaryEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UniqueName")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.CustomerErabliere", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Access")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("ErabliereId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdCustomer")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ErabliereId");

                    b.HasIndex("IdCustomer");

                    b.ToTable("CustomerErablieres");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Documentation", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("File")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileExtension")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Title")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("IdErabliere");

                    b.ToTable("Documentation");
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

                    b.HasIndex(new[] { "D" }, "D_index");

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

                    b.Property<string>("Text")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<short?>("Valeur")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("IdCapteur");

                    b.HasIndex(new[] { "D" }, "D_Index");

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

                    b.Property<string>("CodePostal")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("IndiceOrdre")
                        .HasColumnType("int");

                    b.Property<string>("IpRule")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Note", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("File")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileExtension")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("IdErabliere")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("NoteDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NotificationFilter")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Text")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Title")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("IdErabliere");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Message", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsUser")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Alerte", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Alertes")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.AlerteCapteur", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Capteur", "Capteur")
                        .WithMany("AlertesCapteur")
                        .HasForeignKey("IdCapteur")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Capteur");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.ApiKey", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Customer", "Customer")
                        .WithMany("ApiKeys")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Baril", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Barils")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Capteur", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Capteurs")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.CustomerErabliere", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany()
                        .HasForeignKey("ErabliereId");

                    b.HasOne("ErabliereApi.Donnees.Customer", "Customer")
                        .WithMany("CustomerErablieres")
                        .HasForeignKey("IdCustomer");

                    b.Navigation("Customer");

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Documentation", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Documentations")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Dompeux", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Dompeux")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Donnee", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Donnees")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.DonneeCapteur", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Capteur", "Capteur")
                        .WithMany("DonneesCapteur")
                        .HasForeignKey("IdCapteur")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Capteur");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Note", b =>
                {
                    b.HasOne("ErabliereApi.Donnees.Erabliere", "Erabliere")
                        .WithMany("Notes")
                        .HasForeignKey("IdErabliere")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Erabliere");
                });

            modelBuilder.Entity("Message", b =>
                {
                    b.HasOne("Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId");

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("Conversation", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Capteur", b =>
                {
                    b.Navigation("AlertesCapteur");

                    b.Navigation("DonneesCapteur");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Customer", b =>
                {
                    b.Navigation("ApiKeys");

                    b.Navigation("CustomerErablieres");
                });

            modelBuilder.Entity("ErabliereApi.Donnees.Erabliere", b =>
                {
                    b.Navigation("Alertes");

                    b.Navigation("Barils");

                    b.Navigation("Capteurs");

                    b.Navigation("Documentations");

                    b.Navigation("Dompeux");

                    b.Navigation("Donnees");

                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
