using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class vmspecifcationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VmSpesification",
                columns: table => new
                {
                    VmSpecification_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vm_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vm_cores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vm_memory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vm_storage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VmSpesification", x => x.VmSpecification_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VmSpesification");
        }
    }
}
