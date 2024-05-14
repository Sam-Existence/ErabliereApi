using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionCustomerErablieresForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerErablieres_Erabliere_ErabliereId",
                table: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_CustomerErablieres_ErabliereId",
                table: "CustomerErablieres");

            migrationBuilder.DropColumn(
                name: "ErabliereId",
                table: "CustomerErablieres");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerErablieres_Erabliere_IdErabliere",
                table: "CustomerErablieres",
                column: "IdErabliere",
                principalTable: "Erabliere",
                principalColumn: "Id");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerErablieres",
                table: "CustomerErablieres");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_CustomerErablieres_IdCustomer",
                table: "CustomerErablieres");

            migrationBuilder.AlterColumn<Guid>(
                table: "CustomerErablieres",
                name: "IdCustomer",
                nullable: false);

            migrationBuilder.AlterColumn<Guid>(
                table: "CustomerErablieres",
                name: "IdErabliere",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerErablieres",
                table: "CustomerErablieres",
                columns: ["IdCustomer", "IdErabliere"]);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_IdErabliere",
                table: "CustomerErablieres",
                column: "IdErabliere");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_IdCustomer",
                table: "CustomerErablieres",
                column: "IdCustomer");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerErablieres_Erabliere_IdErabliere",
                table: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_CustomerErablieres_IdErabliere",
                table: "CustomerErablieres");

            migrationBuilder.AddColumn<Guid>(
                name: "ErabliereId",
                table: "CustomerErablieres",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_ErabliereId",
                table: "CustomerErablieres",
                column: "ErabliereId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerErablieres_Erabliere_ErabliereId",
                table: "CustomerErablieres",
                column: "ErabliereId",
                principalTable: "Erabliere",
                principalColumn: "Id");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerErablieres",
                table: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_CustomerErablieres_IdCustomer",
                table: "CustomerErablieres");

            migrationBuilder.AlterColumn<Guid>(
                table: "CustomerErablieres",
                name: "IdCustomer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_IdCustomer",
                table: "CustomerErablieres",
                column: "IdCustomer");

            migrationBuilder.AlterColumn<Guid>(
                table: "CustomerErablieres",
                name: "IdErabliere",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CustomerErablieres");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerErablieres",
                table: "CustomerErablieres",
                column: "Id");
        }
    }
}
