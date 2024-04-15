using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    /// <summary>
    /// Ajout de la table pour suavegarder les positions des graphiques
    /// </summary>
    public partial class AddTablePositionGraph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PositionGraph",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    D = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PX = table.Column<int>(type: "int", nullable: true),
                    PY = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionGraph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionGraph_Erabliere_IdErabliere",
                        column: x => x.IdErabliere,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PositionGraphs_IdErabliere",
                table: "PositionGraph",
                column: "IdErabliere");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PositionGraph");
        }
    }
}
