using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToRappel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rappels_Erabliere_ErabliereId",
                table: "Rappels");

            migrationBuilder.DropIndex(
                name: "IX_Rappels_ErabliereId",
                table: "Rappels");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "Rappels");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rappels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rappels");

            migrationBuilder.AddColumn<Guid>(
                name: "ErabliereId",
                table: "Rappels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rappels_ErabliereId",
                table: "Rappels",
                column: "ErabliereId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rappels_Erabliere_ErabliereId",
                table: "Rappels",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id");
        }
    }
}
