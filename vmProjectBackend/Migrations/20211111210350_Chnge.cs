using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class Chnge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Users_UserId",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_UserId",
                table: "Enrollment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_UserId",
                table: "Enrollment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Users_UserId",
                table: "Enrollment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
