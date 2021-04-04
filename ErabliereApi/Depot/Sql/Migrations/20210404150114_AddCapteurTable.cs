using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    public partial class AddCapteurTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AfficherSectionBaril",
                table: "Erabliere",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Capteurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IdErabliere = table.Column<int>(type: "int", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capteurs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Capteurs");

            migrationBuilder.DropColumn(
                name: "AfficherSectionBaril",
                table: "Erabliere");
        }
    }
}
