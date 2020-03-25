using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.AgentManagement.MsSqlRepositories.Migrations
{
    public partial class AddTokensRequirementsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tokens_requirement",
                schema: "agent_management",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    amount = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens_requirement", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens_requirement",
                schema: "agent_management");
        }
    }
}
