using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    public partial class BonificationRelationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AfficherSectionDompeux",
                table: "Erabliere",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AfficherTrioDonnees",
                table: "Erabliere",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Valeur",
                table: "DonneesCapteur",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

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

            migrationBuilder.AddColumn<bool>(
                name: "AfficherCapteurDashboard",
                table: "Capteurs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ErabliereId",
                table: "Barils",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donnees_ErabliereId",
                table: "Donnees",
                column: "ErabliereId");

            migrationBuilder.CreateIndex(
                name: "IX_Dompeux_ErabliereId",
                table: "Dompeux",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barils_Erabliere_ErabliereId",
                table: "Barils");

            migrationBuilder.DropForeignKey(
                name: "FK_Dompeux_Erabliere_ErabliereId",
                table: "Dompeux");

            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_Erabliere_ErabliereId",
                table: "Donnees");

            migrationBuilder.DropIndex(
                name: "IX_Donnees_ErabliereId",
                table: "Donnees");

            migrationBuilder.DropIndex(
                name: "IX_Dompeux_ErabliereId",
                table: "Dompeux");

            migrationBuilder.DropIndex(
                name: "IX_Barils_ErabliereId",
                table: "Barils");

            migrationBuilder.DropColumn(
                name: "AfficherSectionDompeux",
                table: "Erabliere");

            migrationBuilder.DropColumn(
                name: "AfficherTrioDonnees",
                table: "Erabliere");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Donnees");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Dompeux");

            migrationBuilder.DropColumn(
                name: "AfficherCapteurDashboard",
                table: "Capteurs");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Barils");

            migrationBuilder.AlterColumn<short>(
                name: "Valeur",
                table: "DonneesCapteur",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);
        }
    }
}
