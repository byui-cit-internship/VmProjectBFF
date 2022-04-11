using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class TagBugfix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_ip_address_vm_user_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_ip_address");

            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_tag_vm_user_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_vm_user_instance_vm_template_vm_template_id",
                schema: "vmProject",
                table: "vm_user_instance");

            migrationBuilder.DropForeignKey(
                name: "FK_VmInstanceVswitch_vm_user_instance_vm_instance_id",
                schema: "vmProject",
                table: "VmInstanceVswitch");

            migrationBuilder.DropForeignKey(
                name: "FK_VmInstanceVswitch_vswitch_vswitch_id",
                schema: "vmProject",
                table: "VmInstanceVswitch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VmInstanceVswitch",
                schema: "vmProject",
                table: "VmInstanceVswitch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vm_user_instance",
                schema: "vmProject",
                table: "vm_user_instance");

            migrationBuilder.RenameTable(
                name: "VmInstanceVswitch",
                schema: "vmProject",
                newName: "vm_instance_vswitch",
                newSchema: "vmProject");

            migrationBuilder.RenameTable(
                name: "vm_user_instance",
                schema: "vmProject",
                newName: "vm_instance",
                newSchema: "vmProject");

            migrationBuilder.RenameIndex(
                name: "IX_VmInstanceVswitch_vswitch_id",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                newName: "IX_vm_instance_vswitch_vswitch_id");

            migrationBuilder.RenameIndex(
                name: "IX_VmInstanceVswitch_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                newName: "IX_vm_instance_vswitch_vm_instance_id");

            migrationBuilder.RenameColumn(
                name: "vm_user_instance_vcenter_id",
                schema: "vmProject",
                table: "vm_instance",
                newName: "vm_instance_vcenter_id");

            migrationBuilder.RenameColumn(
                name: "vm_user_instance_expire_date",
                schema: "vmProject",
                table: "vm_instance",
                newName: "vm_instance_expire_date");

            migrationBuilder.RenameColumn(
                name: "vm_user_instance_id",
                schema: "vmProject",
                table: "vm_instance",
                newName: "vm_instance_id");

            migrationBuilder.RenameIndex(
                name: "IX_vm_user_instance_vm_template_id",
                schema: "vmProject",
                table: "vm_instance",
                newName: "IX_vm_instance_vm_template_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vm_instance_vswitch",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                column: "vm_instance_vswitch_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vm_instance",
                schema: "vmProject",
                table: "vm_instance",
                column: "vm_instance_id");

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_vm_template_vm_template_id",
                schema: "vmProject",
                table: "vm_instance",
                column: "vm_template_id",
                principalSchema: "vmProject",
                principalTable: "vm_template",
                principalColumn: "vm_template_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_ip_address_vm_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_ip_address",
                column: "vm_instance_id",
                principalSchema: "vmProject",
                principalTable: "vm_instance",
                principalColumn: "vm_instance_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_tag_vm_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_tag",
                column: "vm_instance_id",
                principalSchema: "vmProject",
                principalTable: "vm_instance",
                principalColumn: "vm_instance_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_vswitch_vm_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                column: "vm_instance_id",
                principalSchema: "vmProject",
                principalTable: "vm_instance",
                principalColumn: "vm_instance_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_vswitch_vswitch_vswitch_id",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                column: "vswitch_id",
                principalSchema: "vmProject",
                principalTable: "vswitch",
                principalColumn: "vswitch_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_vm_template_vm_template_id",
                schema: "vmProject",
                table: "vm_instance");

            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_ip_address_vm_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_ip_address");

            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_tag_vm_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_vswitch_vm_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_vswitch");

            migrationBuilder.DropForeignKey(
                name: "FK_vm_instance_vswitch_vswitch_vswitch_id",
                schema: "vmProject",
                table: "vm_instance_vswitch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vm_instance_vswitch",
                schema: "vmProject",
                table: "vm_instance_vswitch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vm_instance",
                schema: "vmProject",
                table: "vm_instance");

            migrationBuilder.RenameTable(
                name: "vm_instance_vswitch",
                schema: "vmProject",
                newName: "VmInstanceVswitch",
                newSchema: "vmProject");

            migrationBuilder.RenameTable(
                name: "vm_instance",
                schema: "vmProject",
                newName: "vm_user_instance",
                newSchema: "vmProject");

            migrationBuilder.RenameIndex(
                name: "IX_vm_instance_vswitch_vswitch_id",
                schema: "vmProject",
                table: "VmInstanceVswitch",
                newName: "IX_VmInstanceVswitch_vswitch_id");

            migrationBuilder.RenameIndex(
                name: "IX_vm_instance_vswitch_vm_instance_id",
                schema: "vmProject",
                table: "VmInstanceVswitch",
                newName: "IX_VmInstanceVswitch_vm_instance_id");

            migrationBuilder.RenameColumn(
                name: "vm_instance_vcenter_id",
                schema: "vmProject",
                table: "vm_user_instance",
                newName: "vm_user_instance_vcenter_id");

            migrationBuilder.RenameColumn(
                name: "vm_instance_expire_date",
                schema: "vmProject",
                table: "vm_user_instance",
                newName: "vm_user_instance_expire_date");

            migrationBuilder.RenameColumn(
                name: "vm_instance_id",
                schema: "vmProject",
                table: "vm_user_instance",
                newName: "vm_user_instance_id");

            migrationBuilder.RenameIndex(
                name: "IX_vm_instance_vm_template_id",
                schema: "vmProject",
                table: "vm_user_instance",
                newName: "IX_vm_user_instance_vm_template_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VmInstanceVswitch",
                schema: "vmProject",
                table: "VmInstanceVswitch",
                column: "vm_instance_vswitch_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vm_user_instance",
                schema: "vmProject",
                table: "vm_user_instance",
                column: "vm_user_instance_id");

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_ip_address_vm_user_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_ip_address",
                column: "vm_instance_id",
                principalSchema: "vmProject",
                principalTable: "vm_user_instance",
                principalColumn: "vm_user_instance_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vm_instance_tag_vm_user_instance_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_tag",
                column: "vm_instance_id",
                principalSchema: "vmProject",
                principalTable: "vm_user_instance",
                principalColumn: "vm_user_instance_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vm_user_instance_vm_template_vm_template_id",
                schema: "vmProject",
                table: "vm_user_instance",
                column: "vm_template_id",
                principalSchema: "vmProject",
                principalTable: "vm_template",
                principalColumn: "vm_template_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VmInstanceVswitch_vm_user_instance_vm_instance_id",
                schema: "vmProject",
                table: "VmInstanceVswitch",
                column: "vm_instance_id",
                principalSchema: "vmProject",
                principalTable: "vm_user_instance",
                principalColumn: "vm_user_instance_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VmInstanceVswitch_vswitch_vswitch_id",
                schema: "vmProject",
                table: "VmInstanceVswitch",
                column: "vswitch_id",
                principalSchema: "vmProject",
                principalTable: "vswitch",
                principalColumn: "vswitch_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
