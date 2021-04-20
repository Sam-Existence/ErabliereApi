using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    public partial class AjoutDonneeCapteurRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErabliereId",
                table: "Capteurs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DonneesCapteur",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valeur = table.Column<short>(type: "smallint", nullable: false),
                    IdCapteur = table.Column<int>(type: "int", nullable: true),
                    CapteurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonneesCapteur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonneesCapteur_Capteurs_CapteurId",
                        column: x => x.CapteurId,
                        principalTable: "Capteurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capteurs_ErabliereId",
                table: "Capteurs",
                column: "ErabliereId");

            migrationBuilder.CreateIndex(
                name: "IX_DonneesCapteur_CapteurId",
                table: "DonneesCapteur",
                column: "CapteurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Capteurs_Erabliere_ErabliereId",
                table: "Capteurs",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capteurs_Erabliere_ErabliereId",
                table: "Capteurs");

            migrationBuilder.DropTable(
                name: "DonneesCapteur");

            migrationBuilder.DropIndex(
                name: "IX_Capteurs_ErabliereId",
                table: "Capteurs");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Capteurs");
        }
    }
}
