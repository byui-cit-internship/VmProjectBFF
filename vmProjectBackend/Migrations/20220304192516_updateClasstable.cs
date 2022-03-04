using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class updateClasstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "Course",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "ContentLibrary",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Section",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateVm",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentLibrary",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TemplateVm",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Course",
                newName: "description");
        }
    }
}
