using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.Migrations
{
    public partial class RentSubmissionRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_RentSubmissions_RentSubmissionId",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_RentSubmissionId",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "RentSubmissionId",
                table: "Statuses");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RentSubmissions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RentSubmissionStatus",
                columns: table => new
                {
                    AvailableStatusTransitionsId = table.Column<int>(type: "int", nullable: false),
                    RentSubmissionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentSubmissionStatus", x => new { x.AvailableStatusTransitionsId, x.RentSubmissionsId });
                    table.ForeignKey(
                        name: "FK_RentSubmissionStatus_RentSubmissions_RentSubmissionsId",
                        column: x => x.RentSubmissionsId,
                        principalTable: "RentSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentSubmissionStatus_Statuses_AvailableStatusTransitionsId",
                        column: x => x.AvailableStatusTransitionsId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentSubmissions_CategoryId",
                table: "RentSubmissions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RentSubmissions_MarkId",
                table: "RentSubmissions",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_RentSubmissions_UserId",
                table: "RentSubmissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentSubmissionStatus_RentSubmissionsId",
                table: "RentSubmissionStatus",
                column: "RentSubmissionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentSubmissions_AspNetUsers_UserId",
                table: "RentSubmissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentSubmissions_Categories_CategoryId",
                table: "RentSubmissions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentSubmissions_Marks_MarkId",
                table: "RentSubmissions",
                column: "MarkId",
                principalTable: "Marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentSubmissions_AspNetUsers_UserId",
                table: "RentSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RentSubmissions_Categories_CategoryId",
                table: "RentSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RentSubmissions_Marks_MarkId",
                table: "RentSubmissions");

            migrationBuilder.DropTable(
                name: "RentSubmissionStatus");

            migrationBuilder.DropIndex(
                name: "IX_RentSubmissions_CategoryId",
                table: "RentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_RentSubmissions_MarkId",
                table: "RentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_RentSubmissions_UserId",
                table: "RentSubmissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RentSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "RentSubmissionId",
                table: "Statuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_RentSubmissionId",
                table: "Statuses",
                column: "RentSubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_RentSubmissions_RentSubmissionId",
                table: "Statuses",
                column: "RentSubmissionId",
                principalTable: "RentSubmissions",
                principalColumn: "Id");
        }
    }
}
