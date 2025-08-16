using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryMicroservice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnitPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "InventoryItems",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "InventoryItems");
        }
    }
}
