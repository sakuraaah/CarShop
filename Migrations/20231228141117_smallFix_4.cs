using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.Migrations
{
    public partial class smallFix_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentOrder_AspNetUsers_UserId",
                table: "RentOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_RentOrder_RentItems_RentItemId",
                table: "RentOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentOrder",
                table: "RentOrder");

            migrationBuilder.RenameTable(
                name: "RentOrder",
                newName: "RentOrders");

            migrationBuilder.RenameIndex(
                name: "IX_RentOrder_UserId",
                table: "RentOrders",
                newName: "IX_RentOrders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RentOrder_RentItemId",
                table: "RentOrders",
                newName: "IX_RentOrders_RentItemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "BusyTill",
                table: "RentItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RentOrders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentOrders",
                table: "RentOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentOrders_AspNetUsers_UserId",
                table: "RentOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentOrders_RentItems_RentItemId",
                table: "RentOrders",
                column: "RentItemId",
                principalTable: "RentItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentOrders_AspNetUsers_UserId",
                table: "RentOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_RentOrders_RentItems_RentItemId",
                table: "RentOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentOrders",
                table: "RentOrders");

            migrationBuilder.DropColumn(
                name: "BusyTill",
                table: "RentItems");

            migrationBuilder.RenameTable(
                name: "RentOrders",
                newName: "RentOrder");

            migrationBuilder.RenameIndex(
                name: "IX_RentOrders_UserId",
                table: "RentOrder",
                newName: "IX_RentOrder_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RentOrders_RentItemId",
                table: "RentOrder",
                newName: "IX_RentOrder_RentItemId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RentOrder",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentOrder",
                table: "RentOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentOrder_AspNetUsers_UserId",
                table: "RentOrder",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentOrder_RentItems_RentItemId",
                table: "RentOrder",
                column: "RentItemId",
                principalTable: "RentItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
