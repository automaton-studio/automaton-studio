using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Automaton.Studio.Server.Migrations
{
    /// <inheritdoc />
    public partial class Schedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Runners",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 6, 8, 45, 20, 667, DateTimeKind.Local).AddTicks(6545),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 24, 23, 33, 43, 528, DateTimeKind.Local).AddTicks(2381));

            migrationBuilder.AddColumn<string>(
                name: "Application",
                table: "FlowExecutions",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(type: "longtext", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: false),
                    MessageTemplate = table.Column<string>(type: "longtext", nullable: false),
                    Exception = table.Column<string>(type: "longtext", nullable: true),
                    Properties = table.Column<string>(type: "longtext", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventType = table.Column<string>(type: "longtext", nullable: true),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    FlowId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RunnerIds = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ScheduleUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleUsers", x => new { x.ScheduleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ScheduleUsers_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUser_ScheduleId",
                table: "ScheduleUsers",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ScheduleUsers");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropColumn(
                name: "Application",
                table: "FlowExecutions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Runners",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 24, 23, 33, 43, 528, DateTimeKind.Local).AddTicks(2381),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 10, 6, 8, 45, 20, 667, DateTimeKind.Local).AddTicks(6545));
        }
    }
}
