using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddCapteurImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CapteurImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Port = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Identifiant = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MotDePasse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapteurImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapteurImage_Erabliere_IdErabliere",
                        column: x => x.IdErabliere,
                        principalTable: "Erabliere",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CapteurImage_IdErabliere",
                table: "CapteurImage",
                column: "IdErabliere");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CapteurImage");
        }
    }
}
