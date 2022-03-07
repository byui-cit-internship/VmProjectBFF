using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class CreateVmSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "vm_image",
                table: "VmTable",
                newName: "VmName");

            migrationBuilder.AddColumn<string>(
                name: "VmFolder",
                table: "VmTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VmResourcePool",
                table: "VmTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VmFolder",
                table: "VmTable");

            migrationBuilder.DropColumn(
                name: "VmResourcePool",
                table: "VmTable");

            migrationBuilder.RenameColumn(
                name: "VmName",
                table: "VmTable",
                newName: "vm_image");
        }
    }
}
