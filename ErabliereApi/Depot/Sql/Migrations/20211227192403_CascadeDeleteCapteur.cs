using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    public partial class CascadeDeleteCapteur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capteurs_Erabliere_IdErabliere",
                table: "Capteurs");

            migrationBuilder.AddForeignKey(
                name: "FK_Capteurs_Erabliere_IdErabliere",
                table: "Capteurs",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capteurs_Erabliere_IdErabliere",
                table: "Capteurs");

            migrationBuilder.AddForeignKey(
                name: "FK_Capteurs_Erabliere_IdErabliere",
                table: "Capteurs",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id");
        }
    }
}
