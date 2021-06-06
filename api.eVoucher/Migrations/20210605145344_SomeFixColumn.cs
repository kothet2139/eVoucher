using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.eVoucher.Migrations
{
    public partial class SomeFixColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tran_date",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "card_expiry_date",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cvv",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_date",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "card_expiry_date",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "cvv",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "payment_date",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "tran_date",
                table: "Orders");
        }
    }
}
