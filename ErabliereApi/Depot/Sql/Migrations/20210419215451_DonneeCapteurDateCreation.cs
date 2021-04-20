using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErabliereApi.Depot.Sql.Migrations
{
    public partial class DonneeCapteurDateCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "D",
                table: "DonneesCapteur",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "D",
                table: "DonneesCapteur");
        }
    }
}
