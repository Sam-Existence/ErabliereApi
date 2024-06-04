using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddRappel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderDate",
                table: "Notes");

            migrationBuilder.CreateTable(
                name: "Rappels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdErabliere = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ErabliereId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateRappel = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Periodicite = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rappels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rappels_Erabliere_ErabliereId",
                        column: x => x.ErabliereId,
                        principalTable: "Erabliere",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rappels_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rappels_ErabliereId",
                table: "Rappels",
                column: "ErabliereId");

            migrationBuilder.CreateIndex(
                name: "IX_Rappels_NoteId",
                table: "Rappels",
                column: "NoteId",
                unique: true,
                filter: "[NoteId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rappels");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReminderDate",
                table: "Notes",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
