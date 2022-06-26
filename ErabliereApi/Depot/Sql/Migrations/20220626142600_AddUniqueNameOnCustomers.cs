using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <summary>
    /// Ajout une colonne avec représentant un nom unique pour un utilisateur.
    /// Typequement sera la même valeur que le email. 
    /// 
    /// Lorsque l'api est utilisé avec IdentityServer, le nom unique sera
    /// le nom d'utilisateur d'IdentityServer qui n'est pas nécessairement
    /// un email.
    /// </summary>
    public partial class AddUniqueNameOnCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueName",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE Customers SET UniqueName = Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UniqueName",
                table: "Customers",
                column: "UniqueName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_UniqueName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UniqueName",
                table: "Customers");
        }
    }
}
