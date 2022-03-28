using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class MergeAuth1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_membership",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "tag_user",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vlan_vswitch",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_instance_ip_address",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_instance_tag",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_instance_vswitch",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_template_tag",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vswitch_tag",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "group",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vlan",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "ip_address",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_instance",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "tag",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vswitch",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "tag_category",
                schema: "vmProject");

            migrationBuilder.AddColumn<string>(
                name: "ContentLibrary",
                schema: "vmProject",
                table: "course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "vmProject",
                table: "course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Folder",
                schema: "vmProject",
                table: "course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Section",
                schema: "vmProject",
                table: "course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                schema: "vmProject",
                table: "course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateVm",
                schema: "vmProject",
                table: "course",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "VmTables",
                columns: table => new
                {
                    VmTableID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VmFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VmResourcePool = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VmTables", x => x.VmTableID);
                });

            migrationBuilder.CreateTable(
                name: "VmUtilizations",
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
                    table.PrimaryKey("PK_VmUtilizations", x => x.UtilizationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VmDetails");

            migrationBuilder.DropTable(
                name: "VmTables");

            migrationBuilder.DropTable(
                name: "VmUtilizations");

            migrationBuilder.DropColumn(
                name: "ContentLibrary",
                schema: "vmProject",
                table: "course");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "vmProject",
                table: "course");

            migrationBuilder.DropColumn(
                name: "Folder",
                schema: "vmProject",
                table: "course");

            migrationBuilder.DropColumn(
                name: "Section",
                schema: "vmProject",
                table: "course");

            migrationBuilder.DropColumn(
                name: "Semester",
                schema: "vmProject",
                table: "course");

            migrationBuilder.DropColumn(
                name: "TemplateVm",
                schema: "vmProject",
                table: "course");

            migrationBuilder.CreateTable(
                name: "group",
                schema: "vmProject",
                columns: table => new
                {
                    group_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    canvas_group_id = table.Column<int>(type: "int", nullable: false),
                    group_name = table.Column<string>(type: "varchar(45)", nullable: false),
                    section_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group", x => x.group_id);
                    table.ForeignKey(
                        name: "FK_group_section_section_id",
                        column: x => x.section_id,
                        principalSchema: "vmProject",
                        principalTable: "section",
                        principalColumn: "section_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ip_address",
                schema: "vmProject",
                columns: table => new
                {
                    ip_address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ip_address = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    is_ipv6 = table.Column<bool>(type: "bit", nullable: false),
                    subnet_mask = table.Column<byte[]>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ip_address", x => x.ip_address_id);
                });

            migrationBuilder.CreateTable(
                name: "tag_category",
                schema: "vmProject",
                columns: table => new
                {
                    tag_category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_category_description = table.Column<string>(type: "varchar(100)", nullable: true),
                    tag_category_name = table.Column<string>(type: "varchar(45)", nullable: false),
                    tag_category_vcenter_id = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_category", x => x.tag_category_id);
                });

            migrationBuilder.CreateTable(
                name: "vlan",
                schema: "vmProject",
                columns: table => new
                {
                    vlan_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vlan_description = table.Column<string>(type: "varchar(100)", nullable: true),
                    vlan_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vlan", x => x.vlan_id);
                });

            migrationBuilder.CreateTable(
                name: "vm_instance",
                schema: "vmProject",
                columns: table => new
                {
                    vm_instance_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vm_template_id = table.Column<int>(type: "int", nullable: false),
                    vm_instance_vcenter_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    vm_instance_expire_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_instance", x => x.vm_instance_id);
                    table.ForeignKey(
                        name: "FK_vm_instance_vm_template_vm_template_id",
                        column: x => x.vm_template_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_template",
                        principalColumn: "vm_template_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vswitch",
                schema: "vmProject",
                columns: table => new
                {
                    vswitch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vswitch_description = table.Column<string>(type: "varchar(45)", nullable: false),
                    vswitch_name = table.Column<string>(type: "varchar(45)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vswitch", x => x.vswitch_id);
                });

            migrationBuilder.CreateTable(
                name: "group_membership",
                schema: "vmProject",
                columns: table => new
                {
                    group_membership_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_membership", x => x.group_membership_id);
                    table.ForeignKey(
                        name: "FK_group_membership_group_group_id",
                        column: x => x.group_id,
                        principalSchema: "vmProject",
                        principalTable: "group",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_membership_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "vmProject",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                schema: "vmProject",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_category_id = table.Column<int>(type: "int", nullable: false),
                    tag_description = table.Column<string>(type: "varchar(100)", nullable: true),
                    tag_name = table.Column<string>(type: "varchar(45)", nullable: false),
                    tag_vcenter_id = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.tag_id);
                    table.ForeignKey(
                        name: "FK_tag_tag_category_tag_category_id",
                        column: x => x.tag_category_id,
                        principalSchema: "vmProject",
                        principalTable: "tag_category",
                        principalColumn: "tag_category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_instance_ip_address",
                schema: "vmProject",
                columns: table => new
                {
                    vm_instance_ip_address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ip_address_id = table.Column<int>(type: "int", nullable: false),
                    vm_instance_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_instance_ip_address", x => x.vm_instance_ip_address_id);
                    table.ForeignKey(
                        name: "FK_vm_instance_ip_address_ip_address_ip_address_id",
                        column: x => x.ip_address_id,
                        principalSchema: "vmProject",
                        principalTable: "ip_address",
                        principalColumn: "ip_address_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vm_instance_ip_address_vm_instance_vm_instance_id",
                        column: x => x.vm_instance_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_instance",
                        principalColumn: "vm_instance_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vlan_vswitch",
                schema: "vmProject",
                columns: table => new
                {
                    vlan_vswitch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vlan_id = table.Column<int>(type: "int", nullable: false),
                    vswitch_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vlan_vswitch", x => x.vlan_vswitch_id);
                    table.ForeignKey(
                        name: "FK_vlan_vswitch_vlan_vlan_id",
                        column: x => x.vlan_id,
                        principalSchema: "vmProject",
                        principalTable: "vlan",
                        principalColumn: "vlan_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vlan_vswitch_vswitch_vswitch_id",
                        column: x => x.vswitch_id,
                        principalSchema: "vmProject",
                        principalTable: "vswitch",
                        principalColumn: "vswitch_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_instance_vswitch",
                schema: "vmProject",
                columns: table => new
                {
                    vm_instance_vswitch_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vm_instance_id = table.Column<int>(type: "int", nullable: false),
                    vswitch_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_instance_vswitch", x => x.vm_instance_vswitch_id);
                    table.ForeignKey(
                        name: "FK_vm_instance_vswitch_vm_instance_vm_instance_id",
                        column: x => x.vm_instance_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_instance",
                        principalColumn: "vm_instance_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vm_instance_vswitch_vswitch_vswitch_id",
                        column: x => x.vswitch_id,
                        principalSchema: "vmProject",
                        principalTable: "vswitch",
                        principalColumn: "vswitch_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_user",
                schema: "vmProject",
                columns: table => new
                {
                    tag_user_id = table.Column<int>(name: "tag)_user_id", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_user", x => x.tag_user_id);
                    table.ForeignKey(
                        name: "FK_tag_user_tag_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "vmProject",
                        principalTable: "tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tag_user_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "vmProject",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_instance_tag",
                schema: "vmProject",
                columns: table => new
                {
                    vm_instance_tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    vm_instance_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_instance_tag", x => x.vm_instance_tag_id);
                    table.ForeignKey(
                        name: "FK_vm_instance_tag_tag_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "vmProject",
                        principalTable: "tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vm_instance_tag_vm_instance_vm_instance_id",
                        column: x => x.vm_instance_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_instance",
                        principalColumn: "vm_instance_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_template_tag",
                schema: "vmProject",
                columns: table => new
                {
                    vm_template_tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    vm_template_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_template_tag", x => x.vm_template_tag_id);
                    table.ForeignKey(
                        name: "FK_vm_template_tag_tag_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "vmProject",
                        principalTable: "tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vm_template_tag_vm_template_vm_template_id",
                        column: x => x.vm_template_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_template",
                        principalColumn: "vm_template_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vswitch_tag",
                schema: "vmProject",
                columns: table => new
                {
                    vswitch_tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    vswitch_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vswitch_tag", x => x.vswitch_tag_id);
                    table.ForeignKey(
                        name: "FK_vswitch_tag_tag_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "vmProject",
                        principalTable: "tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vswitch_tag_vswitch_vswitch_id",
                        column: x => x.vswitch_id,
                        principalSchema: "vmProject",
                        principalTable: "vswitch",
                        principalColumn: "vswitch_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_group_section_id",
                schema: "vmProject",
                table: "group",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_membership_group_id",
                schema: "vmProject",
                table: "group_membership",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_membership_user_id",
                schema: "vmProject",
                table: "group_membership",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_tag_category_id",
                schema: "vmProject",
                table: "tag",
                column: "tag_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_user_tag_id",
                schema: "vmProject",
                table: "tag_user",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_user_user_id",
                schema: "vmProject",
                table: "tag_user",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vlan_vswitch_vlan_id",
                schema: "vmProject",
                table: "vlan_vswitch",
                column: "vlan_id");

            migrationBuilder.CreateIndex(
                name: "IX_vlan_vswitch_vswitch_id",
                schema: "vmProject",
                table: "vlan_vswitch",
                column: "vswitch_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_vm_template_id",
                schema: "vmProject",
                table: "vm_instance",
                column: "vm_template_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_ip_address_ip_address_id",
                schema: "vmProject",
                table: "vm_instance_ip_address",
                column: "ip_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_ip_address_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_ip_address",
                column: "vm_instance_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_tag_tag_id",
                schema: "vmProject",
                table: "vm_instance_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_tag_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_tag",
                column: "vm_instance_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_vswitch_vm_instance_id",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                column: "vm_instance_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_instance_vswitch_vswitch_id",
                schema: "vmProject",
                table: "vm_instance_vswitch",
                column: "vswitch_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_template_tag_tag_id",
                schema: "vmProject",
                table: "vm_template_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_template_tag_vm_template_id",
                schema: "vmProject",
                table: "vm_template_tag",
                column: "vm_template_id");

            migrationBuilder.CreateIndex(
                name: "IX_vswitch_tag_tag_id",
                schema: "vmProject",
                table: "vswitch_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_vswitch_tag_vswitch_id",
                schema: "vmProject",
                table: "vswitch_tag",
                column: "vswitch_id");
        }
    }
}
