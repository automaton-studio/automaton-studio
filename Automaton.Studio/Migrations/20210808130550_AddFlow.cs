using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Automaton.Studio.Migrations
{
    public partial class AddFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flows_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowWorkflows",
                columns: table => new
                {
                    FlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowWorkflows", x => new { x.FlowId, x.WorkflowId });
                    
                });

            migrationBuilder.CreateIndex(
               name: "IX_Flows_UserId",
               table: "Flows",
               column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowWorkflows_WorkflowId",
                table: "FlowWorkflows",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                        name: "FK_FlowWorkflows_Flows_FlowId",
                        column: "FlowId",
                        table: "FlowWorkflows",
                        principalTable: "Flows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowWorkflows");

            migrationBuilder.DropTable(
                name: "Flows");
        }
    }
}
