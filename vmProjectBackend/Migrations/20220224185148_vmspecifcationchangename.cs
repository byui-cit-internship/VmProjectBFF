using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class vmspecifcationchangename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VmSpesification",
                table: "VmSpesification");

            migrationBuilder.RenameTable(
                name: "VmSpesification",
                newName: "VmSpecification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VmSpecification",
                table: "VmSpecification",
                column: "VmSpecification_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VmSpecification",
                table: "VmSpecification");

            migrationBuilder.RenameTable(
                name: "VmSpecification",
                newName: "VmSpesification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VmSpesification",
                table: "VmSpesification",
                column: "VmSpecification_id");
        }
    }
}
