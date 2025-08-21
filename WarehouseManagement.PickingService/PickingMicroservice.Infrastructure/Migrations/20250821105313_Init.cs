using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickingMicroservice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    OrderNumber = table.Column<string>(type: "text", nullable: true),
                    CustomerName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PickItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PickTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickItem_PickTasks_PickTaskId",
                        column: x => x.PickTaskId,
                        principalTable: "PickTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PickItem_PickTaskId",
                table: "PickItem",
                column: "PickTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickItem");

            migrationBuilder.DropTable(
                name: "PickTasks");
        }
    }
}
