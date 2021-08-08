using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    public partial class FixMissingSymbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barils_Erabliere_ErabliereId",
                table: "Barils");

            migrationBuilder.DropForeignKey(
                name: "FK_Capteurs_Erabliere_ErabliereId",
                table: "Capteurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dompeux_Erabliere_ErabliereId",
                table: "Dompeux");

            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_Erabliere_ErabliereId",
                table: "Donnees");

            migrationBuilder.DropForeignKey(
                name: "FK_DonneesCapteur_Capteurs_CapteurId",
                table: "DonneesCapteur");

            migrationBuilder.DropIndex(
                name: "IX_DonneesCapteur_CapteurId",
                table: "DonneesCapteur");

            migrationBuilder.DropIndex(
                name: "IX_Donnees_ErabliereId",
                table: "Donnees");

            migrationBuilder.DropIndex(
                name: "IX_Dompeux_ErabliereId",
                table: "Dompeux");

            migrationBuilder.DropIndex(
                name: "IX_Capteurs_ErabliereId",
                table: "Capteurs");

            migrationBuilder.DropIndex(
                name: "IX_Barils_ErabliereId",
                table: "Barils");

            migrationBuilder.DropColumn(
                name: "CapteurId",
                table: "DonneesCapteur");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Donnees");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Dompeux");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Capteurs");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Barils");

            migrationBuilder.AddColumn<string>(
                name: "Symbole",
                table: "Capteurs",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DonneesCapteur_IdCapteur",
                table: "DonneesCapteur",
                column: "IdCapteur");

            migrationBuilder.CreateIndex(
                name: "IX_Donnees_IdErabliere",
                table: "Donnees",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_Dompeux_IdErabliere",
                table: "Dompeux",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_Capteurs_IdErabliere",
                table: "Capteurs",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_Barils_IdErabliere",
                table: "Barils",
                column: "IdErabliere");

            migrationBuilder.AddForeignKey(
                name: "FK_Barils_Erabliere_IdErabliere",
                table: "Barils",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // already in previous migrations, don't know why ef add this instruction

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Capteurs_Erabliere_IdErabliere",
            //    table: "Capteurs",
            //    column: "IdErabliere",
            //    principalTable: "Erabliere",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dompeux_Erabliere_IdErabliere",
                table: "Dompeux",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // already in previous migrations, don't know why ef add this instruction

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Donnees_Erabliere_IdErabliere",
            //    table: "Donnees",
            //    column: "IdErabliere",
            //    principalTable: "Erabliere",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                table: "DonneesCapteur",
                column: "IdCapteur",
                principalTable: "Capteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barils_Erabliere_IdErabliere",
                table: "Barils");

            migrationBuilder.DropForeignKey(
                name: "FK_Capteurs_Erabliere_IdErabliere",
                table: "Capteurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dompeux_Erabliere_IdErabliere",
                table: "Dompeux");

            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_Erabliere_IdErabliere",
                table: "Donnees");

            migrationBuilder.DropForeignKey(
                name: "FK_DonneesCapteur_Capteurs_IdCapteur",
                table: "DonneesCapteur");

            migrationBuilder.DropIndex(
                name: "IX_DonneesCapteur_IdCapteur",
                table: "DonneesCapteur");

            migrationBuilder.DropIndex(
                name: "IX_Donnees_IdErabliere",
                table: "Donnees");

            migrationBuilder.DropIndex(
                name: "IX_Dompeux_IdErabliere",
                table: "Dompeux");

            migrationBuilder.DropIndex(
                name: "IX_Capteurs_IdErabliere",
                table: "Capteurs");

            migrationBuilder.DropIndex(
                name: "IX_Barils_IdErabliere",
                table: "Barils");

            migrationBuilder.DropColumn(
                name: "Symbole",
                table: "Capteurs");

            migrationBuilder.AddColumn<int>(
                name: "CapteurId",
                table: "DonneesCapteur",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ErabliereId",
                table: "Donnees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ErabliereId",
                table: "Dompeux",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ErabliereId",
                table: "Capteurs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ErabliereId",
                table: "Barils",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonneesCapteur_CapteurId",
                table: "DonneesCapteur",
                column: "CapteurId");

            migrationBuilder.CreateIndex(
                name: "IX_Donnees_ErabliereId",
                table: "Donnees",
                column: "ErabliereId");

            migrationBuilder.CreateIndex(
                name: "IX_Dompeux_ErabliereId",
                table: "Dompeux",
                column: "ErabliereId");

            migrationBuilder.CreateIndex(
                name: "IX_Capteurs_ErabliereId",
                table: "Capteurs",
                column: "ErabliereId");

            migrationBuilder.CreateIndex(
                name: "IX_Barils_ErabliereId",
                table: "Barils",
                column: "ErabliereId");

            migrationBuilder.AddForeignKey(
                name: "FK_Barils_Erabliere_ErabliereId",
                table: "Barils",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Capteurs_Erabliere_ErabliereId",
                table: "Capteurs",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dompeux_Erabliere_ErabliereId",
                table: "Dompeux",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_Erabliere_ErabliereId",
                table: "Donnees",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonneesCapteur_Capteurs_CapteurId",
                table: "DonneesCapteur",
                column: "CapteurId",
                principalTable: "Capteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
