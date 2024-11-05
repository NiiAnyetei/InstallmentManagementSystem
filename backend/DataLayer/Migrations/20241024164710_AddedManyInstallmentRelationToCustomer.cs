using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedManyInstallmentRelationToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Installments_CustomerId",
                table: "Installments");

            migrationBuilder.CreateIndex(
                name: "IX_Installments_CustomerId",
                table: "Installments",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Installments_CustomerId",
                table: "Installments");

            migrationBuilder.CreateIndex(
                name: "IX_Installments_CustomerId",
                table: "Installments",
                column: "CustomerId",
                unique: true);
        }
    }
}
