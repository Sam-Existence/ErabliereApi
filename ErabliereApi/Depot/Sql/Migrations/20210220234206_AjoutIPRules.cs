using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    /// <summary>
    /// Ajout d'un champs pour stocker les règles d'adresse ip
    /// </summary>
    public partial class AjoutIPRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpRule",
                table: "Erabliere",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpRule",
                table: "Erabliere");
        }
    }
}
