using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class updateColumnSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "section_num",
                table: "Enrollment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "section_num",
                table: "Enrollment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
