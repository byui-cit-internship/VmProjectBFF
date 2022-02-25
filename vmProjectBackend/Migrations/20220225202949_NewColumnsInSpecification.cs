using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class NewColumnsInSpecification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vm_storage",
                table: "VmSpecification",
                newName: "resource_pool");

            migrationBuilder.RenameColumn(
                name: "Vm_name",
                table: "VmSpecification",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Vm_memory",
                table: "VmSpecification",
                newName: "guest_OS");

            migrationBuilder.RenameColumn(
                name: "Vm_image",
                table: "VmSpecification",
                newName: "folder");

            migrationBuilder.RenameColumn(
                name: "Vm_cores",
                table: "VmSpecification",
                newName: "datastore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resource_pool",
                table: "VmSpecification",
                newName: "Vm_storage");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "VmSpecification",
                newName: "Vm_name");

            migrationBuilder.RenameColumn(
                name: "guest_OS",
                table: "VmSpecification",
                newName: "Vm_memory");

            migrationBuilder.RenameColumn(
                name: "folder",
                table: "VmSpecification",
                newName: "Vm_image");

            migrationBuilder.RenameColumn(
                name: "datastore",
                table: "VmSpecification",
                newName: "Vm_cores");
        }
    }
}
