using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alertes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdErabliere = table.Column<int>(type: "int", nullable: true),
                    EnvoyerA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemperatureThresholdLow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemperatureThresholdHight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VacciumThresholdLow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VacciumThresholdHight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NiveauBassinThresholdLow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NiveauBassinThresholdHight = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Barils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DF = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdErabliere = table.Column<int>(type: "int", nullable: true),
                    QE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barils", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dompeux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    T = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DF = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdErabliere = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dompeux", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donnees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    D = table.Column<DateTime>(type: "datetime2", nullable: true),
                    T = table.Column<short>(type: "smallint", nullable: true),
                    NB = table.Column<short>(type: "smallint", nullable: true),
                    V = table.Column<short>(type: "smallint", nullable: true),
                    IdErabliere = table.Column<int>(type: "int", nullable: true),
                    PI = table.Column<int>(type: "int", nullable: true),
                    Nboc = table.Column<int>(type: "int", nullable: false),
                    Iddp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donnees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Erabliere",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erabliere", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Erabliere");
        }
    }
}
