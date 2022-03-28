using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vmProjectBackend.Migrations
{
    public partial class AuthenticationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_token",
                schema: "vmProject",
                columns: table => new
                {
                    access_token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    access_token_value = table.Column<string>(type: "varchar(200)", nullable: false),
                    expire_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_token", x => x.access_token_id);
                    table.ForeignKey(
                        name: "FK_access_token_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "vmProject",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session_token",
                schema: "vmProject",
                columns: table => new
                {
                    session_token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sesion_token_value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    expire_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    access_token_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_token", x => x.session_token_id);
                    table.ForeignKey(
                        name: "FK_session_token_access_token_access_token_id",
                        column: x => x.access_token_id,
                        principalSchema: "vmProject",
                        principalTable: "access_token",
                        principalColumn: "access_token_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_token_user_id",
                schema: "vmProject",
                table: "access_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_session_token_access_token_id",
                schema: "vmProject",
                table: "session_token",
                column: "access_token_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "session_token",
                schema: "vmProject");

            migrationBuilder.DropTable(
                name: "access_token",
                schema: "vmProject");
        }
    }
}
