using Microsoft.EntityFrameworkCore.Migrations;

namespace Automaton.Studio.Migrations
{
    public partial class AddStartupWorkflowId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StartupWorkflowId",
                table: "Flows",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartupWorkflowId",
                table: "Flows");
        }
    }
}
