using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class updateCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VmDetails",
                table: "VmDetails");

            migrationBuilder.RenameTable(
                name: "VmDetails",
                newName: "VmDetail");

            migrationBuilder.AddColumn<string>(
                name: "Folder",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VmDetail",
                table: "VmDetail",
                column: "VmDetailsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VmDetail",
                table: "VmDetail");

            migrationBuilder.DropColumn(
                name: "Folder",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "VmDetail",
                newName: "VmDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VmDetails",
                table: "VmDetails",
                column: "VmDetailsID");
        }
    }
}
