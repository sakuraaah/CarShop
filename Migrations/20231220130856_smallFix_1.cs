using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.Migrations
{
    public partial class smallFix_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_AspNetUsers_UserId",
                table: "BuyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_BodyTypes_BodyTypeId",
                table: "BuyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_Categories_CategoryId",
                table: "BuyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_Colors_ColorId",
                table: "BuyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_Marks_MarkId",
                table: "BuyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItemFeature_BuyItem_BuyItemsId",
                table: "BuyItemFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItemStatus_BuyItem_BuyItemsId",
                table: "BuyItemStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuyItem",
                table: "BuyItem");

            migrationBuilder.RenameTable(
                name: "BuyItem",
                newName: "BuyItems");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_UserId",
                table: "BuyItems",
                newName: "IX_BuyItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_MarkId",
                table: "BuyItems",
                newName: "IX_BuyItems_MarkId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_ColorId",
                table: "BuyItems",
                newName: "IX_BuyItems_ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_CategoryId",
                table: "BuyItems",
                newName: "IX_BuyItems_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_BodyTypeId",
                table: "BuyItems",
                newName: "IX_BuyItems_BodyTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "ImgSrc",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BuyItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuyItems",
                table: "BuyItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItemFeature_BuyItems_BuyItemsId",
                table: "BuyItemFeature",
                column: "BuyItemsId",
                principalTable: "BuyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_AspNetUsers_UserId",
                table: "BuyItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_BodyTypes_BodyTypeId",
                table: "BuyItems",
                column: "BodyTypeId",
                principalTable: "BodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_Categories_CategoryId",
                table: "BuyItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_Colors_ColorId",
                table: "BuyItems",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_Marks_MarkId",
                table: "BuyItems",
                column: "MarkId",
                principalTable: "Marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItemStatus_BuyItems_BuyItemsId",
                table: "BuyItemStatus",
                column: "BuyItemsId",
                principalTable: "BuyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyItemFeature_BuyItems_BuyItemsId",
                table: "BuyItemFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_AspNetUsers_UserId",
                table: "BuyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_BodyTypes_BodyTypeId",
                table: "BuyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_Categories_CategoryId",
                table: "BuyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_Colors_ColorId",
                table: "BuyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_Marks_MarkId",
                table: "BuyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItemStatus_BuyItems_BuyItemsId",
                table: "BuyItemStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuyItems",
                table: "BuyItems");

            migrationBuilder.RenameTable(
                name: "BuyItems",
                newName: "BuyItem");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_UserId",
                table: "BuyItem",
                newName: "IX_BuyItem_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_MarkId",
                table: "BuyItem",
                newName: "IX_BuyItem_MarkId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_ColorId",
                table: "BuyItem",
                newName: "IX_BuyItem_ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_CategoryId",
                table: "BuyItem",
                newName: "IX_BuyItem_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_BodyTypeId",
                table: "BuyItem",
                newName: "IX_BuyItem_BodyTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "ImgSrc",
                table: "AspNetUsers",
                type: "nvarchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BuyItem",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuyItem",
                table: "BuyItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_AspNetUsers_UserId",
                table: "BuyItem",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_BodyTypes_BodyTypeId",
                table: "BuyItem",
                column: "BodyTypeId",
                principalTable: "BodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_Categories_CategoryId",
                table: "BuyItem",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_Colors_ColorId",
                table: "BuyItem",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_Marks_MarkId",
                table: "BuyItem",
                column: "MarkId",
                principalTable: "Marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItemFeature_BuyItem_BuyItemsId",
                table: "BuyItemFeature",
                column: "BuyItemsId",
                principalTable: "BuyItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItemStatus_BuyItem_BuyItemsId",
                table: "BuyItemStatus",
                column: "BuyItemsId",
                principalTable: "BuyItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
