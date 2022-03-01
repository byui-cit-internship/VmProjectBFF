using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class utilizationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VmSpecification");

            migrationBuilder.CreateTable(
                name: "VmUtilization",
                columns: table => new
                {
                    UtilizationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirtualMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VmUtilization", x => x.UtilizationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VmUtilization");

            migrationBuilder.CreateTable(
                name: "VmSpecification",
                columns: table => new
                {
                    VmSpecification_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cdroms_iso_file = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpu_cores_per_socket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpu_count = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    disks_capacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    memory_size_MiB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    placement_datastore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    placement_folder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    placement_resource_pool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    spec_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VmSpecification", x => x.VmSpecification_id);
                });
        }
    }
}
