using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    public partial class MoreCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barils_Erabliere_IdErabliere",
                table: "Barils");

            migrationBuilder.DropForeignKey(
                name: "FK_Dompeux_Erabliere_IdErabliere",
                table: "Dompeux");

            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_Erabliere_IdErabliere",
                table: "Donnees");

            migrationBuilder.AddForeignKey(
                name: "FK_Barils_Erabliere_IdErabliere",
                table: "Barils",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dompeux_Erabliere_IdErabliere",
                table: "Dompeux",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_Erabliere_IdErabliere",
                table: "Donnees",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barils_Erabliere_IdErabliere",
                table: "Barils");

            migrationBuilder.DropForeignKey(
                name: "FK_Dompeux_Erabliere_IdErabliere",
                table: "Dompeux");

            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_Erabliere_IdErabliere",
                table: "Donnees");

            migrationBuilder.AddForeignKey(
                name: "FK_Barils_Erabliere_IdErabliere",
                table: "Barils",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dompeux_Erabliere_IdErabliere",
                table: "Dompeux",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_Erabliere_IdErabliere",
                table: "Donnees",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id");
        }
    }
}
