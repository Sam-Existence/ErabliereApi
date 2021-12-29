using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
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
