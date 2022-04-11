using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class deleteCourseCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "course_code",
                schema: "vmProject",
                table: "course");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "course_code",
                schema: "vmProject",
                table: "course",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "");
        }
    }
}
