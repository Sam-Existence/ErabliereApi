using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <summary>
    /// Ajout des tables et contrainte pour la gestion des droits sur les données
    /// </summary>
    public partial class OwnablePhase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerErablieres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ErabliereId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdCustomer = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Access = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerErablieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerErablieres_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerErablieres_Erabliere_ErabliereId",
                        column: x => x.ErabliereId,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertes_IdErabliere",
                table: "Alertes",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_CustomerId",
                table: "CustomerErablieres",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_ErabliereId",
                table: "CustomerErablieres",
                column: "ErabliereId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alertes_Erabliere_IdErabliere",
                table: "Alertes",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alertes_Erabliere_IdErabliere",
                table: "Alertes");

            migrationBuilder.DropTable(
                name: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_Alertes_IdErabliere",
                table: "Alertes");
        }
    }
}
