using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameClosedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinishedAt",
                table: "Baskets",
                newName: "ClosedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClosedAt",
                table: "Baskets",
                newName: "FinishedAt");
        }
    }
}
