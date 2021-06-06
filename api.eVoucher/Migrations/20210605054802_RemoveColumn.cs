using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.eVoucher.Migrations
{
    public partial class RemoveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "modified_by",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "modified_date",
                table: "PaymentMethod");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "PaymentMethod",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "PaymentMethod",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "modified_by",
                table: "PaymentMethod",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_date",
                table: "PaymentMethod",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
