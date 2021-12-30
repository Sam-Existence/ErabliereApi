using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    public partial class InitialSchemaV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alertes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnvoyerA = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TemperatureThresholdLow = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TemperatureThresholdHight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VacciumThresholdLow = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VacciumThresholdHight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NiveauBassinThresholdLow = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NiveauBassinThresholdHight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Erabliere",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IpRule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IndiceOrdre = table.Column<int>(type: "int", nullable: true),
                    AfficherSectionBaril = table.Column<bool>(type: "bit", nullable: true),
                    AfficherTrioDonnees = table.Column<bool>(type: "bit", nullable: true),
                    AfficherSectionDompeux = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erabliere", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Barils",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DF = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QE = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Q = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Barils_Erabliere_IdErabliere",
                        column: x => x.IdErabliere,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Capteurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AfficherCapteurDashboard = table.Column<bool>(type: "bit", nullable: true),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Symbole = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capteurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Capteurs_Erabliere_IdErabliere",
                        column: x => x.IdErabliere,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dompeux",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    T = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DD = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DF = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dompeux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dompeux_Erabliere_IdErabliere",
                        column: x => x.IdErabliere,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Donnees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    D = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    T = table.Column<short>(type: "smallint", nullable: true),
                    NB = table.Column<short>(type: "smallint", nullable: true),
                    V = table.Column<short>(type: "smallint", nullable: true),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PI = table.Column<int>(type: "int", nullable: true),
                    Nboc = table.Column<int>(type: "int", nullable: false),
                    Iddp = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donnees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donnees_Erabliere_IdErabliere",
                        column: x => x.IdErabliere,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DonneesCapteur",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valeur = table.Column<short>(type: "smallint", nullable: true),
                    D = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IdCapteur = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonneesCapteur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                        column: x => x.IdCapteur,
                        principalTable: "Capteurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barils_IdErabliere",
                table: "Barils",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_Capteurs_IdErabliere",
                table: "Capteurs",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_Dompeux_IdErabliere",
                table: "Dompeux",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_Donnees_IdErabliere",
                table: "Donnees",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_DonneesCapteur_IdCapteur",
                table: "DonneesCapteur",
                column: "IdCapteur");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
        {
            migrationBuilder.DropTable(
                name: "Alertes");

            migrationBuilder.DropTable(
                name: "Barils");

            migrationBuilder.DropTable(
                name: "Dompeux");

            migrationBuilder.DropTable(
                name: "Donnees");

            migrationBuilder.DropTable(
                name: "DonneesCapteur");

            migrationBuilder.DropTable(
                name: "Capteurs");

            migrationBuilder.DropTable(
                name: "Erabliere");
        }
    }
}
