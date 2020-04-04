using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.AgentManagement.MsSqlRepositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "agent_management");

            migrationBuilder.CreateTable(
                name: "agents",
                schema: "agent_management",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(nullable: false),
                    salesforce_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agents", x => x.customer_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agents",
                schema: "agent_management");
        }
    }
}
