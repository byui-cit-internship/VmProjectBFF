using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class canvasTokenToEnrollments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "canvas_token",
                table: "Enrollment",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canvas_token",
                table: "Enrollment");
        }
    }
}
