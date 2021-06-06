using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.eVoucher.Migrations
{
    public partial class AddPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amount",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expiry_date",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_onlyme_usage",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "max_for_me",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "max_to_gift",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "modified_by",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    discount = table.Column<decimal>(type: "numeric", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true),
                    Productid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_Products_Productid",
                        column: x => x.Productid,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_Productid",
                table: "PaymentMethod",
                column: "Productid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "expiry_date",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "image",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "is_onlyme_usage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "max_for_me",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "max_to_gift",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "modified_by",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "Products");
        }
    }
}
