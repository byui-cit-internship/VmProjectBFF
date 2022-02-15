using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class RemoveFKVMTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_VmTable_VmTableID",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_VmTableID",
                table: "Enrollment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_VmTableID",
                table: "Enrollment",
                column: "VmTableID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_VmTable_VmTableID",
                table: "Enrollment",
                column: "VmTableID",
                principalTable: "VmTable",
                principalColumn: "VmTableID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
