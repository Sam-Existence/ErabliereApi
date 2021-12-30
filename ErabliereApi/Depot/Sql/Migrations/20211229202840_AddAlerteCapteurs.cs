using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    public partial class AddAlerteCapteurs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlerteCapteurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCapteur = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnvoyerA = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinVaue = table.Column<short>(type: "smallint", nullable: true),
                    MaxValue = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlerteCapteurs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
        {
            migrationBuilder.DropTable(
                name: "AlerteCapteurs");
        }
    }
}
