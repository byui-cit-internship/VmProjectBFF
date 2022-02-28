using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class detailsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resource_pool",
                table: "VmSpecification",
                newName: "spec_name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "VmSpecification",
                newName: "placement_resource_pool");

            migrationBuilder.RenameColumn(
                name: "guest_OS",
                table: "VmSpecification",
                newName: "placement_folder");

            migrationBuilder.RenameColumn(
                name: "folder",
                table: "VmSpecification",
                newName: "placement_datastore");

            migrationBuilder.RenameColumn(
                name: "datastore",
                table: "VmSpecification",
                newName: "memory_size_MiB");

            migrationBuilder.AddColumn<string>(
                name: "cdroms_iso_file",
                table: "VmSpecification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cpu_cores_per_socket",
                table: "VmSpecification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cpu_count",
                table: "VmSpecification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "disks_capacity",
                table: "VmSpecification",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cdroms_iso_file",
                table: "VmSpecification");

            migrationBuilder.DropColumn(
                name: "cpu_cores_per_socket",
                table: "VmSpecification");

            migrationBuilder.DropColumn(
                name: "cpu_count",
                table: "VmSpecification");

            migrationBuilder.DropColumn(
                name: "disks_capacity",
                table: "VmSpecification");

            migrationBuilder.RenameColumn(
                name: "spec_name",
                table: "VmSpecification",
                newName: "resource_pool");

            migrationBuilder.RenameColumn(
                name: "placement_resource_pool",
                table: "VmSpecification",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "placement_folder",
                table: "VmSpecification",
                newName: "guest_OS");

            migrationBuilder.RenameColumn(
                name: "placement_datastore",
                table: "VmSpecification",
                newName: "folder");

            migrationBuilder.RenameColumn(
                name: "memory_size_MiB",
                table: "VmSpecification",
                newName: "datastore");
        }
    }
}
