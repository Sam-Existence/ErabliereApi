using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    public partial class AddAlerteCapteursRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AlerteCapteurs_IdCapteur",
                table: "AlerteCapteurs",
                column: "IdCapteur");

            migrationBuilder.AddForeignKey(
                name: "FK_AlerteCapteurs_Capteurs_IdCapteur",
                table: "AlerteCapteurs",
                column: "IdCapteur",
                principalTable: "Capteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlerteCapteurs_Capteurs_IdCapteur",
                table: "AlerteCapteurs");

            migrationBuilder.DropIndex(
                name: "IX_AlerteCapteurs_IdCapteur",
                table: "AlerteCapteurs");
        }
    }
}
