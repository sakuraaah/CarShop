using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.Migrations
{
    public partial class RentItemRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RentSubmissionId = table.Column<int>(type: "int", nullable: false),
                    ImgSrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    MarkId = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BodyTypeId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    RentCategoryId = table.Column<int>(type: "int", nullable: false),
                    CarClassId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RentItems_BodyTypes_BodyTypeId",
                        column: x => x.BodyTypeId,
                        principalTable: "BodyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItems_CarClasses_CarClassId",
                        column: x => x.CarClassId,
                        principalTable: "CarClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItems_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItems_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItems_RentCategories_RentCategoryId",
                        column: x => x.RentCategoryId,
                        principalTable: "RentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItems_RentSubmissions_RentSubmissionId",
                        column: x => x.RentSubmissionId,
                        principalTable: "RentSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeatureRentItem",
                columns: table => new
                {
                    FeaturesId = table.Column<int>(type: "int", nullable: false),
                    RentItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureRentItem", x => new { x.FeaturesId, x.RentItemsId });
                    table.ForeignKey(
                        name: "FK_FeatureRentItem_Features_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureRentItem_RentItems_RentItemsId",
                        column: x => x.RentItemsId,
                        principalTable: "RentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentItemStatus",
                columns: table => new
                {
                    AvailableStatusTransitionsId = table.Column<int>(type: "int", nullable: false),
                    RentItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentItemStatus", x => new { x.AvailableStatusTransitionsId, x.RentItemsId });
                    table.ForeignKey(
                        name: "FK_RentItemStatus_RentItems_RentItemsId",
                        column: x => x.RentItemsId,
                        principalTable: "RentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentItemStatus_Statuses_AvailableStatusTransitionsId",
                        column: x => x.AvailableStatusTransitionsId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRentItem_RentItemsId",
                table: "FeatureRentItem",
                column: "RentItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_BodyTypeId",
                table: "RentItems",
                column: "BodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_CarClassId",
                table: "RentItems",
                column: "CarClassId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_CategoryId",
                table: "RentItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_ColorId",
                table: "RentItems",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_MarkId",
                table: "RentItems",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_RentCategoryId",
                table: "RentItems",
                column: "RentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_RentSubmissionId",
                table: "RentItems",
                column: "RentSubmissionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentItems_UserId",
                table: "RentItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItemStatus_RentItemsId",
                table: "RentItemStatus",
                column: "RentItemsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureRentItem");

            migrationBuilder.DropTable(
                name: "RentItemStatus");

            migrationBuilder.DropTable(
                name: "RentItems");
        }
    }
}
