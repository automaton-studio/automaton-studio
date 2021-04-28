﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Automaton.Studio.Migrations
{
    public partial class RemovedElsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "WorkflowDefinitions");

            migrationBuilder.DropTable(
                name: "WorkflowExecutionLogRecords");

            migrationBuilder.DropTable(
                name: "WorkflowInstances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WorkflowInstanceId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDefinitions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DefinitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeleteCompletedInstances = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLatest = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsSingleton = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PersistenceBehavior = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowExecutionLogRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WorkflowInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowExecutionLogRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CancelledAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ContextId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContextType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DefinitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FaultedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    FinishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastExecutedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    WorkflowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_ActivityId",
                table: "Bookmarks",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_ActivityType",
                table: "Bookmarks",
                column: "ActivityType");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_ActivityType_TenantId_Hash",
                table: "Bookmarks",
                columns: new[] { "ActivityType", "TenantId", "Hash" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_Hash",
                table: "Bookmarks",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_TenantId",
                table: "Bookmarks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_WorkflowInstanceId",
                table: "Bookmarks",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_DefinitionId_VersionId",
                table: "WorkflowDefinitions",
                columns: new[] { "DefinitionId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_IsLatest",
                table: "WorkflowDefinitions",
                column: "IsLatest");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_IsPublished",
                table: "WorkflowDefinitions",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_Name",
                table: "WorkflowDefinitions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_TenantId",
                table: "WorkflowDefinitions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinition_Version",
                table: "WorkflowDefinitions",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_ContextId",
                table: "WorkflowInstances",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_ContextType",
                table: "WorkflowInstances",
                column: "ContextType");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_CorrelationId",
                table: "WorkflowInstances",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_CreatedAt",
                table: "WorkflowInstances",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_DefinitionId",
                table: "WorkflowInstances",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_FaultedAt",
                table: "WorkflowInstances",
                column: "FaultedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_FinishedAt",
                table: "WorkflowInstances",
                column: "FinishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_LastExecutedAt",
                table: "WorkflowInstances",
                column: "LastExecutedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_Name",
                table: "WorkflowInstances",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_TenantId",
                table: "WorkflowInstances",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_WorkflowStatus",
                table: "WorkflowInstances",
                column: "WorkflowStatus");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version",
                table: "WorkflowInstances",
                columns: new[] { "WorkflowStatus", "DefinitionId", "Version" });
        }
    }
}
