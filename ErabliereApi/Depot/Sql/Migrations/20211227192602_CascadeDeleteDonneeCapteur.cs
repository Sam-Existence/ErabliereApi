using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    public partial class CascadeDeleteDonneeCapteur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                table: "DonneesCapteur");

            migrationBuilder.AddForeignKey(
                name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                table: "DonneesCapteur",
                column: "IdCapteur",
                principalTable: "Capteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                table: "DonneesCapteur");

            migrationBuilder.AddForeignKey(
                name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                table: "DonneesCapteur",
                column: "IdCapteur",
                principalTable: "Capteurs",
                principalColumn: "Id");
        }
    }
}
