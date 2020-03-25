using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.AgentManagement.MsSqlRepositories.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "images",
                schema: "agent_management",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    customer_id = table.Column<Guid>(nullable: false),
                    document_type = table.Column<short>(nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    content = table.Column<string>(type: "char(64)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_agents",
                        column: x => x.customer_id,
                        principalSchema: "agent_management",
                        principalTable: "agents",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_images_customer_id",
                schema: "agent_management",
                table: "images",
                column: "customer_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images",
                schema: "agent_management");
        }
    }
}
