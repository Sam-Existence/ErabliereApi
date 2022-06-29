using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Sql.Migrations
{
    public partial class AddLinkWithCustomerAndCustoerErabliere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerErablieres_Customers_CustomerId",
                table: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_CustomerErablieres_CustomerId",
                table: "CustomerErablieres");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerErablieres");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_IdCustomer",
                table: "CustomerErablieres",
                column: "IdCustomer");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerErablieres_Customers_IdCustomer",
                table: "CustomerErablieres",
                column: "IdCustomer",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerErablieres_Customers_IdCustomer",
                table: "CustomerErablieres");

            migrationBuilder.DropIndex(
                name: "IX_CustomerErablieres_IdCustomer",
                table: "CustomerErablieres");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "CustomerErablieres",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerErablieres_CustomerId",
                table: "CustomerErablieres",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerErablieres_Customers_CustomerId",
                table: "CustomerErablieres",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
