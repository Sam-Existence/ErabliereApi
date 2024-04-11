using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePositionGraph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PositionGraph",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    D = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PX = table.Column<short>(type: "smallint", nullable: true),
                    PY = table.Column<short>(type: "smallint", nullable: true),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ErabliereId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionGraph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionGraph_Erabliere_ErabliereId",
                        column: x => x.ErabliereId,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "D_Index",
                table: "PositionGraph",
                column: "D");

            migrationBuilder.CreateIndex(
                name: "IX_PositionGraph_ErabliereId",
                table: "PositionGraph",
                column: "ErabliereId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PositionGraph");
        }
    }
}
