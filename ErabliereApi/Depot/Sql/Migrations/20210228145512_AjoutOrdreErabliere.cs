using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    /// <summary>
    /// Ajout d'un champs pour stocker l'ordre d'affichage des érablières
    /// </summary>
    public partial class AjoutOrdreErabliere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IndiceOrdre",
                table: "Erabliere",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndiceOrdre",
                table: "Erabliere");
        }
    }
}
