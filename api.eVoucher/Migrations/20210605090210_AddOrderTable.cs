using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.eVoucher.Migrations
{
    public partial class AddOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_Products_Productid",
                table: "PaymentMethod");

            migrationBuilder.AlterColumn<Guid>(
                name: "Productid",
                table: "PaymentMethod",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    productid = table.Column<Guid>(type: "uuid", nullable: false),
                    card_number = table.Column<string>(type: "text", nullable: true),
                    payment_methodid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    phone_no = table.Column<string>(type: "text", nullable: true),
                    tran_id = table.Column<string>(type: "text", nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    tran_status = table.Column<string>(type: "text", nullable: true),
                    payment_status = table.Column<string>(type: "text", nullable: true),
                    generated_status = table.Column<bool>(type: "boolean", nullable: false),
                    tran_date = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_PaymentMethod_payment_methodid",
                        column: x => x.payment_methodid,
                        principalTable: "PaymentMethod",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Products_productid",
                        column: x => x.productid,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_methodid",
                table: "Orders",
                column: "payment_methodid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_productid",
                table: "Orders",
                column: "productid");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_Products_Productid",
                table: "PaymentMethod",
                column: "Productid",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_Products_Productid",
                table: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "Productid",
                table: "PaymentMethod",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_Products_Productid",
                table: "PaymentMethod",
                column: "Productid",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
