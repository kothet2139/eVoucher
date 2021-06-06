using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.eVoucher.Migrations
{
    public partial class AddPurchaseHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseHistories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promo_code = table.Column<string>(type: "text", nullable: true),
                    qr_code = table.Column<byte[]>(type: "bytea", nullable: true),
                    expiry_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    orderid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistories", x => x.id);
                    table.ForeignKey(
                        name: "FK_PurchaseHistories_Orders_orderid",
                        column: x => x.orderid,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistories_orderid",
                table: "PurchaseHistories",
                column: "orderid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseHistories");
        }
    }
}
