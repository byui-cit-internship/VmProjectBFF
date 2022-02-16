using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class CreateVm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VmDetails",
                columns: table => new
                {
                    VmDetailsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Template_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enrollment_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VmDetails", x => x.VmDetailsID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VmDetails");
        }
    }
}
