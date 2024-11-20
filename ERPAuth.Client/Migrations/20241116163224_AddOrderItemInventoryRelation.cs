using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPAuth.Client.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemInventoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_InventoryId",
                table: "OrderItems",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Inventories_InventoryId",
                table: "OrderItems",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Inventories_InventoryId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_InventoryId",
                table: "OrderItems");
        }
    }
}
