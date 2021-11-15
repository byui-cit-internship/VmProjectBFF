using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class changetheenrollemtn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "teacherId",
                table: "Enrollment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "teacherId",
                table: "Enrollment");
        }
    }
}
