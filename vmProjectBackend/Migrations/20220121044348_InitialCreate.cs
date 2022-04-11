using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "vmProject");

            migrationBuilder.CreateTable(
                name: "course",
                schema: "vmProject",
                columns: table => new
                {
                    course_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_code = table.Column<string>(type: "varchar(15)", nullable: false),
                    course_name = table.Column<string>(type: "varchar(75)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.course_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "vmProject",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "semester",
                schema: "vmProject",
                columns: table => new
                {
                    semester_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    semester_year = table.Column<int>(type: "int", nullable: false),
                    semester_term = table.Column<string>(type: "varchar(20)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semester", x => x.semester_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "vmProject",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(20)", nullable: false),
                    last_name = table.Column<string>(type: "varchar(20)", nullable: false),
                    email = table.Column<string>(type: "varchar(30)", nullable: false),
                    canvas_token = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "vm_template",
                schema: "vmProject",
                columns: table => new
                {
                    vm_template_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vm_template_vcenter_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    vm_template_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    vm_template_access_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_template", x => x.vm_template_id);
                });

            migrationBuilder.CreateTable(
                name: "section",
                schema: "vmProject",
                columns: table => new
                {
                    section_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    semester_id = table.Column<int>(type: "int", nullable: false),
                    section_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.section_id);
                    table.ForeignKey(
                        name: "FK_section_course_course_id",
                        column: x => x.course_id,
                        principalSchema: "vmProject",
                        principalTable: "course",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_section_semester_semester_id",
                        column: x => x.semester_id,
                        principalSchema: "vmProject",
                        principalTable: "semester",
                        principalColumn: "semester_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_template_course",
                schema: "vmProject",
                columns: table => new
                {
                    vm_template_course_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vm_template_id = table.Column<int>(type: "int", nullable: false),
                    vm_course_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_template_course", x => x.vm_template_course_id);
                    table.ForeignKey(
                        name: "FK_vm_template_course_course_vm_course_id",
                        column: x => x.vm_course_id,
                        principalSchema: "vmProject",
                        principalTable: "course",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vm_template_course_vm_template_vm_template_id",
                        column: x => x.vm_template_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_template",
                        principalColumn: "vm_template_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_section_role",
                schema: "vmProject",
                columns: table => new
                {
                    user_section_role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    section_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_section_role", x => x.user_section_role_id);
                    table.ForeignKey(
                        name: "FK_user_section_role_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "vmProject",
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_section_role_section_section_id",
                        column: x => x.section_id,
                        principalSchema: "vmProject",
                        principalTable: "section",
                        principalColumn: "section_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_section_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "vmProject",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_user_instance",
                schema: "vmProject",
                columns: table => new
                {
                    vm_user_instance_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_section_role_id = table.Column<int>(type: "int", nullable: false),
                    vm_template_id = table.Column<int>(type: "int", nullable: false),
                    vm_user_instance_vcenter_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    vm_user_instance_expire_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vm_user_instance", x => x.vm_user_instance_id);
                    table.ForeignKey(
                        name: "FK_vm_user_instance_user_section_role_user_section_role_id",
                        column: x => x.user_section_role_id,
                        principalSchema: "vmProject",
                        principalTable: "user_section_role",
                        principalColumn: "user_section_role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vm_user_instance_vm_template_vm_template_id",
                        column: x => x.vm_template_id,
                        principalSchema: "vmProject",
                        principalTable: "vm_template",
                        principalColumn: "vm_template_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_section_course_id",
                schema: "vmProject",
                table: "section",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_semester_id",
                schema: "vmProject",
                table: "section",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_section_role_role_id",
                schema: "vmProject",
                table: "user_section_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_section_role_section_id",
                schema: "vmProject",
                table: "user_section_role",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_section_role_user_id",
                schema: "vmProject",
                table: "user_section_role",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_template_course_vm_course_id",
                schema: "vmProject",
                table: "vm_template_course",
                column: "vm_course_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_template_course_vm_template_id",
                schema: "vmProject",
                table: "vm_template_course",
                column: "vm_template_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_user_instance_user_section_role_id",
                schema: "vmProject",
                table: "vm_user_instance",
                column: "user_section_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_vm_user_instance_vm_template_id",
                schema: "vmProject",
                table: "vm_user_instance",
                column: "vm_template_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vm_template_course",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_user_instance",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "user_section_role",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "vm_template",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "role",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "section",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "user",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "course",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "semester",
                schema: "vmProject");
        }
    }
}
