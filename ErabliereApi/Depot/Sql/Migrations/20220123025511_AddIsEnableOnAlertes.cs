using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations;
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
public partial class AddIsEnableOnAlertes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsEnable",
            table: "Alertes",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsEnable",
            table: "AlerteCapteurs",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsEnable",
            table: "Alertes");

        migrationBuilder.DropColumn(
            name: "IsEnable",
            table: "AlerteCapteurs");
    }
}
